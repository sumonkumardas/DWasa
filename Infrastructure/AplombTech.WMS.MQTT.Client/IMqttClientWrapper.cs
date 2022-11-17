using System.Collections.Generic;

namespace AplombTech.MQTTLib
{
    public interface IMqttClientWrapper
    {
        void MakeConnection();
        string Publish(string messgeTopic, string publishMessage);
        string Subscribe(string messgeTopic);
        void Subscribe(IEnumerable<string> messgeTopics);
    }
}