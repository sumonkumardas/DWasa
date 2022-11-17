using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.JsonParser.Entity
{
    public class MotorValue
    {
        public string MotorUid { get; set; }
        public bool Auto { get; set; }
        public bool Controllable { get; set; }
        [JsonProperty("Motor_Status")]
        public string MotorStatus { get; set; }
        [JsonProperty("Last_Command")]
        public string LastCommand { get; set; }
        [JsonProperty("Last_Command_Time")]
        public string LastCommandTime { get; set; }
    }
}
