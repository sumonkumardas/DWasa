using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.MQTT.Client.ConnectionHandling
{
    public interface IMqttConnectionHandler
    {
        void InitializeMqttConnection();
    }
}
