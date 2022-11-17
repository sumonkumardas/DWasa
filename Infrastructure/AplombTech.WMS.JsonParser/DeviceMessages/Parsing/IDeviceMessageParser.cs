using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.JsonParser.DeviceMessages.Parsing
{
    public interface IDeviceMessageParser<out TMessage> where TMessage : DeviceMessage
    {
        TMessage ParseMessage(string messageString);
    }   
}
