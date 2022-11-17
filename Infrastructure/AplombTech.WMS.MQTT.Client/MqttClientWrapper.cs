using AplombTech.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AplombTech.WMS.MQTT.Client;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using log4net;
using System.Reflection;

namespace AplombTech.MQTTLib
{
  public class MqttClientWrapper : IMqttClientWrapper
  {
    #region delegate event
    #region MqttMsg-Publish-Received-Notification
    public delegate void NotifyMqttMessageReceivedDelegate(MQTTEventArgs customEventArgs);
    public event NotifyMqttMessageReceivedDelegate NotifyMqttMessageReceivedEvent;
    #endregion

    #region MqttMsg-Published-Notification
    public delegate void NotifyMqttMsgPublishedDelegate(MQTTEventArgs customEventArgs);
    public event NotifyMqttMsgPublishedDelegate NotifyMqttMsgPublishedEvent;
    #endregion

    #region MqttMsg-Subscribed-Notification
    public delegate void NotifyMqttMsgSubscribedDelegate(MQTTEventArgs customEventArgs);
    public event NotifyMqttMsgSubscribedDelegate NotifyMqttMsgSubscribedEvent;
    #endregion

    #endregion

    readonly object locker = new object();
    private static readonly ILog Log =
      LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    #region constructor
    public MqttClientWrapper(bool isSSL)
    {
      IsSSL = isSSL;

    }
    public MqttClientWrapper()
    {

    }

    public void Connect()
    {
      MakeConnection();
    }

    #endregion

    #region Properties
    public string WillTopic { get; set; }
    public bool IsSSL { get; private set; }
    public MqttClient DhakaWasaMQTT { get; private set; }
    public string ClientResponce { get; private set; }
    #endregion

    #region PUBLIC METHODS
    public void MakeConnection()
    {
      try
      {
        if (DhakaWasaMQTT == null || !DhakaWasaMQTT.IsConnected)
        {
          if (IsSSL)
          {
            BrokerConnectionWithCertificate();
          }
          else
          {
            BrokerConnectionWithoutCertificate();
          }
          DefinedMQTTCommunicationEvents();
        }
      }
      catch (Exception ex)
      {
        Log.Error(string.Format("Could not stablished connection to MQ broker: {0}", ex.Message));

        //don't leave the client connected
        if (DhakaWasaMQTT != null && DhakaWasaMQTT.IsConnected)
        {
          try
          {
            DhakaWasaMQTT.Disconnect();
          }
          catch
          {
            Log.Error(string.Format("Could not disconnect to MQ broker: {0}", ex.Message));
          }
        }
        MakeConnection();
      }
    }
    public string Publish(string messgeTopic, string publishMessage)
    {
      if (DhakaWasaMQTT == null)
      {
        MakeConnection();
      }

        if (DhakaWasaMQTT != null)
      {
        try
        {
          lock (locker)
          {
            ushort msgId = DhakaWasaMQTT.Publish(messgeTopic, // topic
                              Encoding.UTF8.GetBytes(publishMessage), // message body
                              MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, // QoS level
                              false);
            Log.Info("Published called!");
            return "Success";
          }
        }
        catch (Exception ex)
        {
              Log.Warn("Error while publishing: " + ex.Message, ex);
        }
      }
      return "Fail";
    }
    public string Subscribe(string messgeTopic)
    {
      if (DhakaWasaMQTT != null)
      {
        ushort msgId = DhakaWasaMQTT.Subscribe(new string[] { messgeTopic },
             new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE }
             );
        Log.Info(string.Format("Subscription to topic {0}", messgeTopic));
      }
      return "Success";
    }
    public void Subscribe(IEnumerable<string> messgeTopics)
    {
      foreach (var item in messgeTopics)
        Subscribe(item);
    }
    #endregion

    #region EVENT HANDLER
    private void client_MqttMsgPublished(object sender, MqttMsgPublishedEventArgs e)
    {
      NotifyMessage("MqttMsgPublished", e.IsPublished.ToString(), string.Empty);
      Log.Info(string.Format("Mqtt-Msg-Published to topic {0}", e.IsPublished.ToString()));
      ClientResponce = "Success";
    }
    private void client_MqttMsgSubscribed(object sender, MqttMsgSubscribedEventArgs e)
    {
      NotifyMessage("MqttMsgSubscribed", e.MessageId.ToString(), string.Empty);
      Log.Info(string.Format("Mqtt-Msg-Subscribed to topic {0}", e.MessageId.ToString()));
    }
    private void client_MqttMsgUnsubscribed(object sender, MqttMsgUnsubscribedEventArgs e)
    {
      ClientResponce = "Success";
    }
    private void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {
      NotifyMessage("MqttMsgPublishReceived", Encoding.UTF8.GetString(e.Message), e.Topic.ToString());
      Log.Info(string.Format("Mqtt-Msg-Publish-Received to topic {0}", e.Topic.ToString()));
    }
    private void client_ConnectionClosed(object sender, EventArgs e)
    {
      if (!(sender as MqttClient).IsConnected || DhakaWasaMQTT == null)
      {
        HandleReconnect();
      }
      Logger.Log("Connection has been closed");
      Log.Info("Connection has been closed");
    }
    private bool client_RemoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
      return true;
      // logic for validation here
    }
    #endregion

    #region Delegate and event implementation
    private void NotifyMessage(string NotifyType, string receivedMessage, string receivedTopic)
    {
      if (NotifyType == "MqttMsgPublishReceived")
      {
        InvokeEvents<NotifyMqttMessageReceivedDelegate>(receivedMessage, receivedTopic, NotifyMqttMessageReceivedEvent);
      }

      if (NotifyType == "MqttMsgPublished")
      {
        InvokeEvents<NotifyMqttMsgPublishedDelegate>(receivedMessage, receivedTopic, NotifyMqttMsgPublishedEvent);
      }

      if (NotifyType == "MqttMsgSubscribed")
      {
        InvokeEvents<NotifyMqttMsgSubscribedDelegate>(receivedMessage, receivedTopic, NotifyMqttMsgSubscribedEvent);
      }
    }
    private static void InvokeEvents<T>(string receivedMessage, string receivedTopic, T eventDelegate)
    {
      if (eventDelegate != null)
      {
        var customEventArgs = new MQTTEventArgs(receivedMessage, receivedTopic);
        ((Delegate)(object)eventDelegate).DynamicInvoke(customEventArgs);
      }
    }
    #endregion

    #region PRIVATE METHODS
    private void DefinedMQTTCommunicationEvents()
    {
      DhakaWasaMQTT.MqttMsgPublished += client_MqttMsgPublished;//publish
      DhakaWasaMQTT.MqttMsgSubscribed += client_MqttMsgSubscribed;//subscribe confirmation
      DhakaWasaMQTT.MqttMsgUnsubscribed += client_MqttMsgUnsubscribed;
      DhakaWasaMQTT.MqttMsgPublishReceived += client_MqttMsgPublishReceived;//received message.
      DhakaWasaMQTT.ConnectionClosed += client_ConnectionClosed;

      //ushort submsgId = DhakaWasaMQTT.Subscribe(new string[] { "/configuration", "/command", "/feedback" },
      //                  new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,
      //                          MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

    }
    private void BrokerConnectionWithCertificate()
    {
      DhakaWasaMQTT = new MqttClient(GetBrokerAddress(), MqttSettings.MQTT_BROKER_DEFAULT_SSL_PORT, true, new X509Certificate(Resource.ca), null, MqttSslProtocols.TLSv1_2, client_RemoteCertificateValidationCallback);
      ConnectToBroker("kanok", "kanok");
    }
    private void ConnectToBroker(string username, string password)
    {
      DhakaWasaMQTT.Connect(GetClientId(), username, password, false, GetBrokerKeepAlivePeriod());
      Log.Info("Broker Connection successful");
    }
    private void BrokerConnectionWithoutCertificate()
    {
      DhakaWasaMQTT = new MqttClient(GetBrokerAddress(), GetBrokerPort(), false, null, null, MqttSslProtocols.None, null);
      //ConnectToBroker();
      ConnectToBroker("kanok", "kanok");
    }
    private void LocalBrokerConnection()
    {
      DhakaWasaMQTT = new MqttClient(GetBrokerAddress());
      ConnectToBroker();
    }
    private void ConnectToBroker()
    {
      DhakaWasaMQTT.Connect(GetClientId(), null, null, false, GetBrokerKeepAlivePeriod());
    }
    private void HandleReconnect()
    {
      MakeConnection();
    }
    private string GetBrokerAddress()
    {
      if (ConfigurationManager.AppSettings["BrokerAddress"] == null)
      {
        return "192.168.11.225";
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
    private UInt16 GetBrokerKeepAlivePeriod()
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
    #endregion
  }

  public class MQTTEventArgs : EventArgs
  {
    public MQTTEventArgs(string receivedMessage, string receivedTopic)
    {
      _receivedMessage = receivedMessage;
      _receivedTopic = receivedTopic;
    }
    private string _receivedMessage;

    public string ReceivedMessage
    {
      get { return _receivedMessage; }
      set { _receivedMessage = value; }
    }

    private string _receivedTopic;
    public string ReceivedTopic
    {
      get { return _receivedTopic; }
      set { _receivedTopic = value; }
    }
  }
}
