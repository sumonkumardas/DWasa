using AplombTech.WMS.JsonParser.DeviceMessages.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.WMS.MQTT.Client.PumpStationMessages.Processing;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace AplombTech.WMS.MQTT.Client.EventListening
{
    public class MqttEventListener : IMqttEventListener
    {
        private readonly IMessageProcessor _messageProcessor;

        public MqttEventListener(IMessageProcessor messageProcessor)
        {
            _messageProcessor = messageProcessor;
        }

        public void OnMqttMsgPublished(object sender, MqttMsgPublishedEventArgs e)
        {
        }

        public void OnMqttMsgSubscribed(object sender, MqttMsgSubscribedEventArgs e)
        {
        }

        public void OnMqttMsgUnsubscribed(object sender, MqttMsgUnsubscribedEventArgs e)
        {
        }

        public void OnMqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            _messageProcessor.ProcessMessage(e.Topic, Encoding.UTF8.GetString(e.Message));
        }

        public void OnConnectionClosed(object sender, EventArgs e)
        {
        }
    }
}
