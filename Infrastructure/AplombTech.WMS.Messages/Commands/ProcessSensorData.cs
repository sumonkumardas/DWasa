using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Messages.Commands
{
    public class ProcessSensorData : ICommand
    {
        public int SensorDataLogId { get; set; }
        public string Topic { get; set; }
        public string Message { get; set; }
        public DateTime LoggedAtSensor { get; set; }
    }
}
