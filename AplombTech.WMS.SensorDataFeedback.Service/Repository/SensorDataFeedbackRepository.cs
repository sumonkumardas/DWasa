using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.WMS.Persistence.Repositories;
using AplombTech.WMS.Persistence.UnitOfWorks;
using AplombTech.WMS.SensorDataFeedback.Mqtt;
using AplombTech.WMS.SensorDataFeedback.Service.Model;
using AplombTech.WMS.SensorDataLogBoundedContext.Repositories;
using AplombTech.WMS.SensorDataLogBoundedContext.UnitOfWorks;

namespace AplombTech.WMS.SensorDataFeedback.Service.Repository
{
  public static class SensorDataFeedbackRepository
  {
    static SensorDataFeedbackRepository()
    {
      log4net.Config.XmlConfigurator.Configure();
    }
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    public static async Task SendSensorDataFeedback()
    {
      using (WMSUnitOfWork wmsUow = new WMSUnitOfWork())
      {
        ProcessRepository repoProcess = new ProcessRepository(wmsUow.CurrentObjectContext);
        var pumpStationDatas = repoProcess.GetAllPumpStationLastRecivedDate();

        foreach (var pumpStation in pumpStationDatas)
        {
          if (pumpStation.Key != null)
          {
            var model = new SensorDataFeedbackModel()
            {
              LastDataReceived = pumpStation.Value,
              PumpStation_Id = pumpStation.Key
            };
            var configMinutes = Int32.Parse(ConfigurationManager.AppSettings["IntervalInMinutes"]);

            //if (model.LastDataReceived.AddMinutes(configMinutes).Ticks >= DateTime.Now.Ticks)
            {

              M2MMqttMessageModel publishModel = new M2MMqttMessageModel
              {
                MessgeTopic = $"wasa/{pumpStation.Key.Trim()}/in/sensor_data_feedback",
                PublishMessage = Newtonsoft.Json.JsonConvert.SerializeObject(model)
              };

              try
              {
                publishModel.PublishMessageStatus = MqttService.MqttClientInstance(false)
                  .Publish(publishModel.MessgeTopic, publishModel.PublishMessage);
                if (publishModel.PublishMessageStatus == "Success")
                {
                  await Task.Factory.StartNew(() => log.Info("Sensor data feedback successfully sent for pumpstation uuid = " + pumpStation.Key));
                  
                }
                else
                {
                  await Task.Factory.StartNew(() => log.Info(
                    "Sensor data feedback received failed. Sever status = " + publishModel.PublishMessageStatus));
                  
                }
              }
              catch (Exception ex)
              {
                await Task.Factory.StartNew(() => log.Info("Sensor data feedback failed to send. Error = " + ex.ToString()));
              }
            }
          }
        }

        
      }
    }
  }
}
