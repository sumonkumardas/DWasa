using AplombTech.WMS.JsonParser.DeviceMessages.Parsing;
using AplombTech.WMS.JsonParser.Topics.Classification;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.MQTT.Client.PumpStationMessages.Processing
{
    public class MessageProcessor : IMessageProcessor
    {
        private readonly ITopicClassifier _topicClassifier;
        private readonly IMessageParserFactory _messageParserFactory;
        private readonly IBus _bus;

        public MessageProcessor(ITopicClassifier topicClassifier, IMessageParserFactory messageParserFactory, IBus bus)
        {
            _topicClassifier = topicClassifier;
            _messageParserFactory = messageParserFactory;
            _bus = bus;
        }

        public void ProcessMessage(string topic, string message)
        {
            var topicType = _topicClassifier.GetTopicType(topic);
            var messageParser = _messageParserFactory.CreateMessageParser(topicType);
            var deviceMessage = messageParser.ParseMessage(message);
            _bus.Send(message);
        }
    }
}
