using AplombTech.WMS.Domain.Areas;
using AplombTech.WMS.Domain.Sensors;
using AplombTech.WMS.JsonParser;
using AplombTech.WMS.JsonParser.Entity;
using NakedObjects.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.WMS.Domain.Devices;
using AplombTech.WMS.Domain.Motors;
using AplombTech.WMS.JsonParser.DeviceMessages;
using AplombTech.WMS.AreaRepositories;
using AplombTech.WMS.Domain.SummaryData;
using AplombTech.WMS.SensorDataLogBoundedContext.Repositories;
using AplombTech.WMS.SensorDataLogBoundedContext.UnitOfWorks;

namespace AplombTech.WMS.DataProcessRepository
{
  //this process repository is not used anymore I think
  //to-do check later if i am right
  //AplombTech.WMS.Persistence.Repositories ProcessRepository is used instead
  public class ProcessRepository : AbstractFactoryAndRepository
    {
        #region Injected Services
        public AreaRepository AreaRepository { set; protected get; }
        #endregion

        
        
        //public DataLog LogData(string topic, string message)
        //{
        //    DateTime loggedAtTime = DateTime.MinValue;
        //    int pumpStationId = 0;

        //    try
        //    {
        //        if (topic == "wasa/sensor_data")
        //            loggedAtTime = JsonManager.GetSensorLoggedAtTime(message);
        //        if (topic == "wasa/configuration")
        //            loggedAtTime = JsonManager.GetConfigurationLoggedAtTime(message);

        //        pumpStationId = JsonManager.GetPumpStationIDFromJson(message);
        //    }
        //    catch (Exception)
        //    {               
        //        return null;
        //    }
            
        //    DataLog sensorLogData = GetDataLog(topic, loggedAtTime, pumpStationId);

        //    if (sensorLogData == null)
        //    {
        //        DataLog data = CreateDataLog(topic, message, loggedAtTime, pumpStationId);
        //        return data;
        //    }

        //    return sensorLogData;
        //}
        private DataLog GetDataLog(string topic, DateTime loggedAtSensor, int stationId)
        {
            using (SDLUnitOfWork uow = new SDLUnitOfWork())
            {
                SDLRepository repo = new SDLRepository(uow.CurrentObjectContext);
                DataLog dataLog = repo.GetDataLog(topic, loggedAtSensor, stationId);
                return dataLog;
            }
            

            
        }
        public DataLog CreateDataLog(string topic, string message, DateTime loggedAtSensor, int stationId)
        {
            using (SDLUnitOfWork uow = new SDLUnitOfWork())
            {
                SDLRepository repo = new SDLRepository(uow.CurrentObjectContext);
                DataLog data = repo.CreateDataLog(topic,message,loggedAtSensor,stationId);
                uow.CurrentObjectContext.SaveChanges();
                return data;
            }
        }

        public void UpdateDataLog(long dataLogId, DataLog.ProcessingStatusEnum status, string remarks)
        {
            using (SDLUnitOfWork uow = new SDLUnitOfWork())
            {
                SDLRepository repo = new SDLRepository(uow.CurrentObjectContext);
                DataLog data = repo.UpdateDataLog(dataLogId,status,remarks);
                uow.CurrentObjectContext.SaveChanges();
            }
        }

        public void CreateNewSensorData(SensorValue sensorValue, DateTime loggedAt, Sensor sensor)
        {
            SensorData data = null;
            using (SDLUnitOfWork uow = new SDLUnitOfWork())
            {
                SDLRepository repo = new SDLRepository(uow.CurrentObjectContext);
                data = repo.CreateNewSensorData(sensorValue, loggedAt,sensor);            
                uow.CurrentObjectContext.SaveChanges();
            }
            if (sensor is FlowSensor || sensor is EnergySensor)
            {
                //DO NOTHING
            }
            else
            {
                UpdateLastDataOfSensor(data.Value, data.ProcessAt, sensor);
            }
        }        
        private void UpdateLastDataOfSensor(decimal value, DateTime loggedAt, Sensor sensor)
        {
            if (sensor.LastDataReceived != null)
            {
                if (sensor.LastDataReceived < loggedAt)
                {                   
                    sensor.CurrentValue = value;                    
                    sensor.LastDataReceived = loggedAt;
                }
            }
            else
            {
                sensor.CurrentValue = value;
                sensor.LastDataReceived = loggedAt;
            }
        }

       public void CreateNewMotorData(MotorValue data, DateTime loggedAt, Motor motor)
        {
            using (SDLUnitOfWork uow = new SDLUnitOfWork())
            {
                SDLRepository repo = new SDLRepository(uow.CurrentObjectContext);
                MotorData motorData = new MotorData();
                motorData.MotorStatus = data.MotorStatus;
                motorData.LastCommand = data.LastCommand;
                motorData.LastCommandTime = data.LastCommandTime;
                motorData.Auto = data.Auto;
                motorData.LoggedAt = loggedAt;
                motorData.ProcessAt = DateTime.Now;
                motorData.MotorId = motor.MotorID;
                repo.CreateNewMotorData(motorData);
                uow.CurrentObjectContext.SaveChanges();
                UpdateLastDataOfMotor(data, motorData.ProcessAt, motor);
            }
        }
        private void UpdateLastDataOfMotor(MotorValue data, DateTime loggedAt, Motor motor)
        {
            motor.Auto = data.Auto;
            motor.Controllable = data.Controllable;
            motor.MotorStatus = data.MotorStatus;
            motor.LastDataReceived = loggedAt;
            motor.LastCommand = data.LastCommand;
            motor.LastCommandTime = data.LastCommandTime;

        }

        public void CreateUnderThresoldData(decimal value,Sensor sensor)
        {
            UnderThresoldData underThresold = Container.NewTransientInstance<UnderThresoldData>();
            underThresold.Sensor = sensor;
            underThresold.Value = value;
            underThresold.LoggedAt = DateTime.Now;
            underThresold.Unit = sensor.UnitName;
            Container.Persist(ref underThresold);
        }
    }
}
