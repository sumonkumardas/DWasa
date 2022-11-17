using AplombTech.WMS.JsonParser.Topics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.JsonParser.DeviceMessages.Parsing
{
    public interface IMessageParserFactory
    {
        IDeviceMessageParser<DeviceMessage> CreateMessageParser(TopicType topic);
    }
}
