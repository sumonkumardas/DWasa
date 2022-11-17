using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.WMS.SensorDataFeedback.Service.Jobs;
using Quartz;
using Quartz.Impl;

namespace AplombTech.WMS.SensorDataFeedback.Service
{
  public class SensorDataFeedbackJobScheduler
  {
    public static async Task Start()
    {
      var scheduler = await new StdSchedulerFactory().GetScheduler();
      await scheduler.Start();
      IJobDetail job = CreateJob();
      
      ITrigger trigger = CreateTrigger();
      
      await scheduler.ScheduleJob(job, trigger);
    }

    private static IJobDetail CreateJob()
    {
      var jobName = $"SendSensorDataFeedbackJob";
      var jobGroup = "SendSensorDataFeedbackJobGroup";
      return JobBuilder.Create<SensorDataFeedbackJob>()
        .WithIdentity(jobName, jobGroup)
        .Build();
    }

    private static ITrigger CreateTrigger()
    {
      var triggerName = $"SendSensorDataFeedbackTrigger";
      var triggerGroup = "SendSensorDataFeedbackTriggerGroup";
      var configMinutes = Int32.Parse(ConfigurationManager.AppSettings["IntervalInMinutes"]);

      var triggerBuilder = TriggerBuilder.Create().WithIdentity(triggerName, triggerGroup);
      var trigger = triggerBuilder
        .StartNow()
        .WithSimpleSchedule(x => x
          .WithIntervalInMinutes(configMinutes)
          .RepeatForever())
        .Build();

      return trigger;
    }
  }
}
