using AplombTech.WMS.Domain.Sensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.JsonParser.Entity
{
  public class SensorEntity
  {
    public string SensorType { get; set; }
    public string Uuid { get; set; }
    public decimal MinValue { get; set; }
    public double MaxValue { get; set; }
    public string DataType { get; set; }
    public string Model { get; set; }
    public string Version { get; set; }
    public string Unit { get; set; }
  }
}
