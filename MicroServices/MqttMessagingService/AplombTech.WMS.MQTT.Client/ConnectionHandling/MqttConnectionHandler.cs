using AplombTech.WMS.MQTT.Client.Configuring;
using AplombTech.WMS.MQTT.Client.EventListening;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.WMS.Utility;
using uPLibrary.Networking.M2Mqtt;

namespace AplombTech.WMS.MQTT.Client.ConnectionHandling
{
    public class MqttConnectionHandler : IMqttConnectionHandler
    {
        private readonly MqttConfiguration _config;
        private readonly IMqttEventListener _mqttEventListener;

        private MqttClient MqttClient { get; set; }

        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public MqttConnectionHandler(MqttConfiguration config, IMqttEventListener mqttEventListener)
        {
            _config = config;
            _mqttEventListener = mqttEventListener;
        }

        public void InitializeMqttConnection()
        {
            Log.Info("MQTT listener is going to start");
            MakeConnection();
        }

        private void BrokerConnectionWithoutCertificate()
        {
            MqttClient = new MqttClient(_config.BrokerAddress, _config.BrokerPort, false, null,
                null, MqttSslProtocols.None, null);
            ConnectToBroker();
        }

        private void ConnectToBroker()
        {
            MqttClient.Connect(_config.ClientId, null, null, false,
                _config.BrokerKeepAlivePeriod);
            Log.Info("MQTT Client is connected");
        }

        private void DefinedMqttCommunicationEvents()
        {
            MqttClient.ConnectionClosed += ConnectionClosed_MQTT;

            MqttClient.MqttMsgPublished += _mqttEventListener.OnMqttMsgPublished;
            MqttClient.MqttMsgSubscribed += _mqttEventListener.OnMqttMsgSubscribed;
            MqttClient.MqttMsgUnsubscribed += _mqttEventListener.OnMqttMsgUnsubscribed;
            MqttClient.MqttMsgPublishReceived += _mqttEventListener.OnMqttMsgPublishReceived;
            MqttClient.ConnectionClosed += _mqttEventListener.OnConnectionClosed;

            var topics = _config.Topics;
            var qosLevels = Enumerable.Repeat(_config.QosLevel, topics.Length).ToArray();

            MqttClient.Subscribe(topics, qosLevels);
        }

        private void Reconnect()
        {
            MakeConnection();
        }

        private void MakeConnection()
        {
            try
            {
                if (MqttClient == null || !MqttClient.IsConnected)
                {
                    if (_config.IsSsl)
                    {
                        //BrokerConnectionWithCertificate();
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
                Log.Error("Could not established connection to MQTT broker - " + ex.Message);
#if DEBUG
                EmailSender.SendEmail("ashraful.islam@aplombtechbd.com", "SmartMeter:Could not stablished connection to MQTT broker", ex.Message);

#else
                EmailSender.SendEmail("ashraful.islam@aplombtechbd.com", "SmartMeter:Could not stablished connection to MQTT broker", ex.Message);
#endif
        //don't leave the client connected
        if (MqttClient != null && MqttClient.IsConnected)
                {
                    try
                    {
                        MqttClient.Disconnect();
                    }
                    catch
                    {
                        Log.Error(string.Format("Could not disconnect to MQTT broker: {1}", ex.Message));
                    }
                }
                Log.Error(string.Format("Could not disconnect to MQTT broker: {1}", ex.Message));
                MakeConnection();
            }
        }

        #region EVENT HANDLER

        private void ConnectionClosed_MQTT(object sender, EventArgs e)
        {
            if (!((MqttClient)sender).IsConnected || MqttClient == null)
            {
                Reconnect();
            }
        }

        #endregion
    }
}
