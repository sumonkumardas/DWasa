using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.JsonParser.Topics.Classification
{
    public interface ITopicClassifier
    {
        TopicType GetTopicType(string topic);
    }
}
