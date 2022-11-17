using AplombTech.WMS.JsonParser.Topics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.JsonParser.DeviceMessages.Parsing
{
	public class MessageParserFactory : IMessageParserFactory
	{
		public IDeviceMessageParser<DeviceMessage> CreateMessageParser(TopicType topic)
		{
			switch (topic)
			{
				case TopicType.SensorData:
					return new SensorMessageParser();
				case TopicType.VfdData:
					return new VfdMessageParser();
				case TopicType.Configuration:
					return new ConfigurationMessageParser();
				default:
					throw new Exception("Invlid Token Type");
			}
		}
	}
}
