using AplombTech.WMS.Domain.Motors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.JsonParser.Entity
{
  public class MotorEntity
  {
    public string MotorType { get; set; }
    public string Uuid { get; set; }
    public bool Auto { get; set; }
    public bool Controllable { get; set; }
  }
}
