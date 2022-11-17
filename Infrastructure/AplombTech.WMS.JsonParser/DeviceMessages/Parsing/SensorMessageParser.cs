using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.WMS.JsonParser.DeviceMessages.Exceptions;

namespace AplombTech.WMS.JsonParser.DeviceMessages.Parsing
{
    public class SensorMessageParser : IDeviceMessageParser<SensorMessage>
    {
        public SensorMessage ParseMessage(string messageString)
        {
            try
            {
                SensorMessage messageObject = JsonManager.GetSensorObject(messageString);
                return messageObject;
            }
            catch (Exception)
            {                
                throw new SensorMessageException(); 
            }
            
        }
    }
}
