using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.JsonParser.Topics.Classification
{
	public class TopicClassifier : ITopicClassifier
	{
		public TopicType GetTopicType(string topic)
		{
			if (topic.StartsWith("wasa/") && topic.EndsWith("/configuration"))
				return TopicType.Configuration;
			else if (topic.StartsWith("wasa/") && topic.EndsWith("/sensor_data"))
				return TopicType.SensorData;
			else if (topic.StartsWith("wasa/") && topic.EndsWith("/vfd_data"))
				return TopicType.VfdData;
			else
				throw new InvalidTopicException();
		}
	}
}
