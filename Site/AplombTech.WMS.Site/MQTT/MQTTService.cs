using AplombTech.MQTTLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Site.MQTT
{
    public class MQTTService
    {
        private static MqttClientWrapper instance = null;
        //Lock synchronization object
        private static object syncLock = new object();
        public static MqttClientWrapper MQTTClientInstance(bool isSSL)
        {
            lock (syncLock)
            {
                if (instance == null)
                {
                    instance = new MqttClientWrapper(isSSL);
                    instance.NotifyMqttMessageReceivedEvent += new MqttClientWrapper.NotifyMqttMessageReceivedDelegate(PublishReceivedMessage_NotifyEvent);

                    instance.NotifyMqttMsgPublishedEvent += new MqttClientWrapper.NotifyMqttMsgPublishedDelegate(PublishedMessage_NotifyEvent);

                    instance.NotifyMqttMsgSubscribedEvent += new MqttClientWrapper.NotifyMqttMsgSubscribedDelegate(SubscribedMessage_NotifyEvent);
                }

                return instance;
            }

            
        }
        static void PublishReceivedMessage_NotifyEvent(MQTTEventArgs customEventArgs)
        {
            //if (customEventArgs.ReceivedTopic == CommandType.Configuration.ToString())
            //{
            //    new JsonManager().JsonProcess(customEventArgs.ReceivedMessage);
            //}
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
