using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.JsonParser.Entity
{
  public class PumpStationEntity
  {
    [JsonProperty("PumpStation_Id")]
    public string PumpStation_UuId { get; set; }
    public string ConfigureDateTime { get; set; }
  }
}
