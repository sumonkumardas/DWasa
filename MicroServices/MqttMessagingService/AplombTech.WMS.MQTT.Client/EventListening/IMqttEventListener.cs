using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace AplombTech.WMS.MQTT.Client.EventListening
{
    public interface IMqttEventListener
    {
        void OnMqttMsgPublished(object sender, MqttMsgPublishedEventArgs e);
        void OnMqttMsgSubscribed(object sender, MqttMsgSubscribedEventArgs e);
        void OnMqttMsgUnsubscribed(object sender, MqttMsgUnsubscribedEventArgs e);
        void OnMqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e);
        void OnConnectionClosed(object sender, EventArgs e);
    }
}
