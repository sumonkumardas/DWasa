using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.WMS.JsonParser.DeviceMessages.Exceptions;
using Newtonsoft.Json;

namespace AplombTech.WMS.JsonParser.DeviceMessages.Parsing
{
    public class ConfigurationMessageParser : IDeviceMessageParser<ConfigurationMessageEntity>
    {
        public ConfigurationMessageEntity ParseMessage(string messageString)
        {
            try
            {
        ConfigurationMessageEntity messageObject = JsonConvert.DeserializeObject<ConfigurationMessageEntity>(messageString);

        return messageObject;
            }
            catch (Exception ex)
            {
                
                throw new ConfigurationMessageException();
            }
            
        }
    }
}
