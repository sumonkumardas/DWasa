using AplombTech.WMS.JsonParser.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.JsonParser.DeviceMessages
{
    public class SensorMessage : DeviceMessage
    {
        public SensorMessage()
        {
            Sensors = new List<SensorValue>();
            Motors = new List<MotorValue>();
        }
        public bool SensorDataComplete { get; set; }
        public int LogCount { get; set; }
        public IList<SensorValue> Sensors { get; set; }
        public IList<MotorValue> Motors { get; set; }
    }
}
