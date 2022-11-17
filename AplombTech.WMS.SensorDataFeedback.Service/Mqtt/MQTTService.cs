using AplombTech.MQTTLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.SensorDataFeedback.Mqtt
{
    public class MqttService
    {
        private static MqttClientWrapper instance = null;
        //Lock synchronization object
        private static readonly object _syncLock = new object();
        public static MqttClientWrapper MqttClientInstance(bool isSsl)
        {
            lock (_syncLock)
            {
                if (instance == null)
                {
                    instance = new MqttClientWrapper(isSsl);
                    instance.NotifyMqttMessageReceivedEvent += new MqttClientWrapper.NotifyMqttMessageReceivedDelegate(PublishReceivedMessage_NotifyEvent);

                    instance.NotifyMqttMsgPublishedEvent += new MqttClientWrapper.NotifyMqttMsgPublishedDelegate(PublishedMessage_NotifyEvent);

                    instance.NotifyMqttMsgSubscribedEvent += new MqttClientWrapper.NotifyMqttMsgSubscribedDelegate(SubscribedMessage_NotifyEvent);
                }

                return instance;
            }

            
        }
        static void PublishReceivedMessage_NotifyEvent(MQTTEventArgs customEventArgs)
        {
        }
        static void PublishedMessage_NotifyEvent(MQTTEventArgs customEventArgs)
        {
            string msg = customEventArgs.ReceivedMessage;

        }
        static void SubscribedMessage_NotifyEvent(MQTTEventArgs customEventArgs)
        {
            string msg = customEventArgs.ReceivedMessage;
        }
    }
}
