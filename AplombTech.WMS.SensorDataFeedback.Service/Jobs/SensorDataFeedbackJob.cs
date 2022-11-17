using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.WMS.SensorDataFeedback.Service.Repository;
using Quartz;

namespace AplombTech.WMS.SensorDataFeedback.Service.Jobs
{
  public class SensorDataFeedbackJob : IJob
  {
    public Task Execute(IJobExecutionContext context)
    {
      return SensorDataFeedbackRepository.SendSensorDataFeedback();
    }
  }
}
