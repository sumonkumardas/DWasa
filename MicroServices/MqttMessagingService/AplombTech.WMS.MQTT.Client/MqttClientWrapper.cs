using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace AplombTech.WMS.MQTT.Client
{
    public class MqttClientWrapper
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
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        readonly object locker = new object();

        #region constructor
        public MqttClientWrapper(bool isSSL)
        {
            IsSSL = isSSL;
        }
        public MqttClientWrapper()
        {

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
                log.Error("Could not stablished connection to MQTT broker - " + ex.Message);

                //don't leave the client connected
                if (DhakaWasaMQTT != null && DhakaWasaMQTT.IsConnected)
                {
                    try
                    {
                        DhakaWasaMQTT.Disconnect();
                    }
                    catch
                    {
                        log.Error(string.Format("Could not disconnect to MQTT broker: {1}", ex.Message));
                    }
                }
                throw new Exception("Could not stablished connection to MQTT broker");
            }
        }
        public string Publish(string messgeTopic, string publishMessage)
        {
            if (DhakaWasaMQTT != null)
            {
                try
                {
                    lock (locker)
                    {
                        ushort msgId = DhakaWasaMQTT.Publish(messgeTopic, // topic
                                          Encoding.UTF8.GetBytes(publishMessage), // message body
                                          MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, // QoS level
                                          true);
                    }
                }
                catch (Exception ex)
                {
                    //    log.Warn("Error while publishing: " + ex.Message, ex);
                }
            }
            return "Success";
        }
        public string Subscribe(string messgeTopic)
        {
            if (DhakaWasaMQTT != null)
            {
                ushort msgId = DhakaWasaMQTT.Subscribe(new string[] { messgeTopic },
                     new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE }
                     );
                log.Info(string.Format("Subscription to topic {0}", messgeTopic));
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
            //log.Info(string.Format("Mqtt-Msg-Published to topic {0}", e.IsPublished.ToString()));
            ClientResponce = "Success";
        }
        private void client_MqttMsgSubscribed(object sender, MqttMsgSubscribedEventArgs e)
        {
            NotifyMessage("MqttMsgSubscribed", e.MessageId.ToString(), string.Empty);
            log.Info(string.Format("Mqtt-Msg-Subscribed to topic {0}", e.MessageId.ToString()));
        }
        private void client_MqttMsgUnsubscribed(object sender, MqttMsgUnsubscribedEventArgs e)
        {
            ClientResponce = "Success";
        }
        private void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            NotifyMessage("MqttMsgPublishReceived", Encoding.UTF8.GetString(e.Message), e.Topic.ToString());
            //log.Info("Message received from topic '" + e.Topic.ToString() + "' and message is '" + Encoding.UTF8.GetString(e.Message) + "'");
        }
        private void client_ConnectionClosed(object sender, EventArgs e)
        {
            if (!(sender as MqttClient).IsConnected || DhakaWasaMQTT == null)
            {
                HandleReconnect();
            }
            log.Info("Connection has been closed");
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

            ushort submsgId = DhakaWasaMQTT.Subscribe(new string[] { "wasa/configuration", "/command", "/feedback", "wasa/sensor_data" },
                              new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,
                                      MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

        }
        private void BrokerConnectionWithCertificate()
        {
            //SmartHomeMQTT = new MqttClient(brokerAddress, MqttSettings.MQTT_BROKER_DEFAULT_SSL_PORT, true, new X509Certificate(Resource.ca), null, MqttSslProtocols.TLSv1_2, client_RemoteCertificateValidationCallback);
            DhakaWasaMQTT.Connect(GetClientId(), "mosharraf", "mosharraf", false, GetBrokerKeepAlivePeriod());
        }
        private void BrokerConnectionWithoutCertificate()
        {
            DhakaWasaMQTT = new MqttClient(GetBrokerAddress(), GetBrokerPort(), false, null, null, MqttSslProtocols.None, null);
            ConnectToBroker();
        }
        private void LocalBrokerConnection()
        {
            DhakaWasaMQTT = new MqttClient(GetBrokerAddress());
            ConnectToBroker();
        }
        private void ConnectToBroker()
        {
            DhakaWasaMQTT.Connect(GetClientId(), null, null, false, GetBrokerKeepAlivePeriod());
            log.Info("MQTT Client is connected");
        }
        private void HandleReconnect()
        {
            MakeConnection();
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
