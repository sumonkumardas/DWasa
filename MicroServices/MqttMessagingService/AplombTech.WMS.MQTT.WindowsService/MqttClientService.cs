using AplombTech.WMS.Domain.Motors;
using AplombTech.WMS.Domain.Sensors;
using AplombTech.WMS.JsonParser;
using AplombTech.WMS.JsonParser.Topics;
using AplombTech.WMS.JsonParser.Topics.Classification;
using AplombTech.WMS.JsonParser.DeviceMessages;
using AplombTech.WMS.JsonParser.DeviceMessages.Parsing;
using AplombTech.WMS.JsonParser.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using AplombTech.WMS.AreaRepositories;
using AplombTech.WMS.Domain.Alerts;
using AplombTech.WMS.Messages.Commands;
using AplombTech.WMS.MQTT.Data.Processor;
using AplombTech.WMS.Persistence.Repositories;
using AplombTech.WMS.Persistence.UnitOfWorks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using AplombTech.WMS.SensorDataLogBoundedContext.UnitOfWorks;
using AplombTech.WMS.SensorDataLogBoundedContext.Repositories;
using AplombTech.WMS.Utility;

namespace AplombTech.WMS.MQTT.WindowsService
{
  public class MqttClientService
  {
    #region Injected Services

    #endregion


    public enum JsonMessageType
    {
      configuration,
      sensordata,
      feedback
    }
    private MqttClient DhakaWasaMqtt { get; set; }
    private Timer _Timer { get; set; }
    private bool IsSsl { get; set; }
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    public void MqttClientInstance(bool isSSL)
    {
      IsSsl = isSSL;
      MakeConnection();
      // InitiateTimer();
    }

    private void InitiateTimer()
    {
      _Timer = new Timer(10000 * 6); // Set up the timer for 3 seconds
                                     //
                                     // Type "_timer.Elapsed += " and press tab twice.
                                     //
      _Timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
      _Timer.Enabled = true;
    }

    void _timer_Elapsed(object sender, ElapsedEventArgs e)
    {
      try
      {
        CheckDataConnection();
      }
      catch (Exception ex)
      {
      }

    }

    private void BrokerConnectionWithoutCertificate()
    {
      DhakaWasaMqtt = new MqttClient(GetBrokerAddress(), GetBrokerPort(), false, null, null, MqttSslProtocols.None, null);
      ConnectToBroker("kanok", "kanok");
    }
    private void BrokerConnectionWithCertificate()
    {
      DhakaWasaMqtt = new MqttClient(GetBrokerAddress(), MqttSettings.MQTT_BROKER_DEFAULT_SSL_PORT, true, new X509Certificate(Resource.ca), null, MqttSslProtocols.TLSv1_2, client_RemoteCertificateValidationCallback);
      ConnectToBroker("kanok", "kanok");
    }

    public bool client_RemoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
      return true;
      // logic for validation here
    }

    private string GetBrokerAddress()
    {
      if (ConfigurationManager.AppSettings["BrokerAddress"] == null)
      {
        return string.Empty;
      }
      return ConfigurationManager.AppSettings["BrokerAddress"].ToString();
    }
    private int GetBrokerPort()
    {
      if (ConfigurationManager.AppSettings["BrokerPort"] == null)
      {
        return 1883;
      }
      return Convert.ToInt32(ConfigurationManager.AppSettings["BrokerPort"]);
    }
    private ushort GetBrokerKeepAlivePeriod()
    {
      if (ConfigurationManager.AppSettings["BrokerKeepAlivePeriod"] == null)
      {
        return 3600;
      }
      return Convert.ToUInt16(ConfigurationManager.AppSettings["BrokerKeepAlivePeriod"]);
    }
    private string GetClientId()
    {
      if (ConfigurationManager.AppSettings["BrokerAccessClientId"] == null)
      {
        string clientId = Guid.NewGuid().ToString();
        return clientId;
      }
      return ConfigurationManager.AppSettings["BrokerAccessClientId"].ToString();
    }
    private void ConnectToBroker()
    {
      DhakaWasaMqtt.Connect(GetClientId(), null, null, false, GetBrokerKeepAlivePeriod());
      log.Info("MQTT Client is connected");
    }
    private void ConnectToBroker(string username, string password)
    {
      DhakaWasaMqtt.Connect(GetClientId(), username, password, true, GetBrokerKeepAlivePeriod());
      log.Info("MQTT Client is connected via SSL");
    }

    public void DisconnectBroker()
    {
      if (DhakaWasaMqtt != null && DhakaWasaMqtt.IsConnected)
      {
        try
        {
          DhakaWasaMqtt.Disconnect();
          log.Info("Mqtt client has ben disconnected");
        }
        catch (Exception ex)
        {
          log.Error(string.Format("Could not disconnect to MQTT broker: {0}", ex.Message));
        }
      }
    }
    private void DefinedMqttCommunicationEvents()
    {
      DhakaWasaMqtt.MqttMsgPublished += PublishedMessage_MQTT;//publish
      DhakaWasaMqtt.MqttMsgSubscribed += SubscribedMessage_MQTT;//subscribe confirmation
      DhakaWasaMqtt.MqttMsgUnsubscribed += UnsubscribedMessage_MQTT;
      DhakaWasaMqtt.MqttMsgPublishReceived += ReceivedMessage_MQTT;//received message.
      DhakaWasaMqtt.ConnectionClosed += ConnectionClosed_MQTT;

      ushort submsgId = DhakaWasaMqtt.Subscribe(new string[] { "wasa/+/out/configuration", "wasa/+/out/sensor_data", "wasa/+/out/vfd_data" },
                        new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,
                                      MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE});
      log.Info("subscribed!!");

    }
    private void HandleReconnect()
    {
      MakeConnection();
    }
    private void MakeConnection()
    {
      try
      {
        if (DhakaWasaMqtt == null || !DhakaWasaMqtt.IsConnected)
        {
          if (IsSsl)
          {
            BrokerConnectionWithCertificate();
          }
          else
          {
            BrokerConnectionWithoutCertificate();
          }
          DefinedMqttCommunicationEvents();
        }
      }
      catch (Exception ex)
      {
        log.Error("Could not stablished connection to MQTT broker - " + ex.Message);

        //don't leave the client connected
        if (DhakaWasaMqtt != null && DhakaWasaMqtt.IsConnected)
        {
          try
          {
            DhakaWasaMqtt.Disconnect();
          }
          catch
          {
            log.Error(string.Format("Could not disconnect to MQTT broker: {1}", ex.Message));
          }
        }
        //throw new Exception("Could not stablished connection to MQTT broker");
        MakeConnection();
      }
    }

    private void CheckDataConnection()
    {
      SendMailForPumpStation();
    }

    private void SendMailForPumpStation()
    {

      CheckAndMail("35");
      CheckAndMail("45");
      CheckAndMail("65");
      CheckAndMail("75");
    }

    private void CheckAndMail(string sensorId)
    {
      using (WMSUnitOfWork wmsUow = new WMSUnitOfWork())
      {
        ProcessRepository repoProcess = new ProcessRepository(wmsUow.CurrentObjectContext);
        var sensor = repoProcess.GetSensorByUuid(sensorId);
        if (sensor.LastDataReceived.HasValue && sensor.LastDataReceived.Value.AddMinutes(1) < DateTime.Now)
        {
#if DEBUG
          EmailSender.SendEmail("ashraful.islam@aplombtechbd.com",
            "Data sending stopped for " + sensor.PumpStation.Name,
            "(Local Deploy)WMS:Data sending is off for " + sensor.PumpStation.Name + ". Last datarecived on " +
            sensor.LastDataReceived);
#else
                EmailSender.SendEmail("ashraful.islam@aplombtechbd.com",
                    "ashraful.islam@aplombtechbd.com", "Data sending stopped for " + sensor.PumpStation.Name,
                    "WMS:Data sending is off for " + sensor.PumpStation.Name + ". Last datarecived on " +
                    sensor.LastDataReceived);
#endif
        }
      }
    }


    #region EVENT HANDLER
    private void PublishedMessage_MQTT(object sender, MqttMsgPublishedEventArgs e)
    {
      //NotifyMessage("MqttMsgPublished", e.IsPublished.ToString(), string.Empty);
      //log.Info(string.Format("Mqtt-Msg-Published to topic {0}", e.IsPublished.ToString()));
      //ClientResponce = "Success";
    }
    private void SubscribedMessage_MQTT(object sender, MqttMsgSubscribedEventArgs e)
    {
      //NotifyMessage("MqttMsgSubscribed", e.MessageId.ToString(), string.Empty);
      log.Info(string.Format("Mqtt-Msg-Subscribed to topic {0}", e.MessageId.ToString()));
    }
    private void UnsubscribedMessage_MQTT(object sender, MqttMsgUnsubscribedEventArgs e)
    {
      //ClientResponce = "Success";
    }
    private void ReceivedMessage_MQTT(object sender, MqttMsgPublishEventArgs e)
    {
      //log.Info("msg received");
      string message = Encoding.UTF8.GetString(e.Message);
      string topic = e.Topic.ToString();
      DataProcessor processor = new DataProcessor(ServiceBus.Bus);
      processor.ProcessData(topic, message);
    }
    private void ConnectionClosed_MQTT(object sender, EventArgs e)
    {
      DhakaWasaMqtt = null;
      if (!(sender as MqttClient).IsConnected || DhakaWasaMqtt == null)
      {
        HandleReconnect();
      }
      //log.Info("Connection has been closed");
    }
    #endregion
  }
}
