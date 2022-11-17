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
using Topshelf;

namespace AplombTech.WMS.SensorDataFeedback.ConsoleApp
{
  public static class Program
  {
    private static Task<IScheduler> _scheduler;
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public static void Main(string[] args)
    {
      log4net.Config.XmlConfigurator.Configure();
      log.Info("Starting scheduler.");
      var rc = HostFactory.Run(x =>
      {
        x.Service<SensorDataFeedbackJobServiceInstance>(s =>
        {
          s.ConstructUsing(name => new ScheduleJobServiceInstance());
          s.WhenStarted(async tc => await tc.Start());
          s.WhenStopped(async tc => await tc.Stop());
        });
        x.RunAsLocalSystem();

        x.SetDescription("Wasa Management System - Schedule Job Service");
        x.SetDisplayName("Wasa System - Schedule Job Service");
        x.SetServiceName("SensorDataFeedbackScheduleJobService");
      });

      var exitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode());
      Environment.ExitCode = exitCode;
    }
  

    

  }
}
