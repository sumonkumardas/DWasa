using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.MQTT.Client.Configuring
{
    public class MqttConfiguration
    {
        public string BrokerAddress { get; set; }
        public int BrokerPort { get; set; }
        public ushort BrokerKeepAlivePeriod { get; set; }
        public string ClientId { get; set; }
        public bool IsSsl { get; set; }
        public string[] Topics { get; set; }
        public byte QosLevel { get; set; }
    }
}
