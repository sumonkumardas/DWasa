using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.WMS.SensorDataFeedback.Service;
using AplombTech.WMS.SensorDataFeedback.Service.Jobs;
using Quartz;
using Quartz.Impl;

namespace AplombTech.WMS.SensorDataFeedback.ConsoleApp
{
  public class ScheduleJobServiceInstance : SensorDataFeedbackJobServiceInstance
  {
    public override async Task Start()
    {
      //RunProgramRunExample().GetAwaiter().GetResult();
      await SensorDataFeedbackJobScheduler.Start();
    }

    public override async Task Stop()
    {
    }

    private static async Task RunProgramRunExample()
    {
      try
      {
        // Grab the Scheduler instance from the Factory

        StdSchedulerFactory factory = new StdSchedulerFactory();
        IScheduler scheduler = await factory.GetScheduler();

        // and start it off
        await scheduler.Start();

        IJobDetail job = CreateJob();

        ITrigger trigger = CreateTrigger();


        // Tell quartz to schedule the job using our trigger
        await scheduler.ScheduleJob(job, trigger);

      }
      catch (SchedulerException se)
      {
        Console.WriteLine(se);
      }
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
