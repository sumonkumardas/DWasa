using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.SensorDataFeedback.ConsoleApp
{
  public abstract class SensorDataFeedbackJobServiceInstance
  {
    public abstract Task Start();
    public abstract Task Stop();
  }
}
