using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.SensorDataFeedback.Service.Model
{
  public class SensorDataFeedbackModel
  {
    public string PumpStation_Id { get; set; }
    public DateTime LastDataReceived { get; set; }
  }
}
