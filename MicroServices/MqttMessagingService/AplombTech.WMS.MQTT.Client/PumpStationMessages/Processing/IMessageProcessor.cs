using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.MQTT.Client.PumpStationMessages.Processing
{
    public interface IMessageProcessor
    {
        void ProcessMessage(string topic, string message);
    }
}
