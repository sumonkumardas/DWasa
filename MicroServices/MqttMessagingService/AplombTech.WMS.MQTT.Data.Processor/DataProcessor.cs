using System;
using System.IO;
using AplombTech.WMS.Domain.Alerts;
using AplombTech.WMS.Domain.Motors;
using AplombTech.WMS.Domain.Sensors;
using AplombTech.WMS.JsonParser;
using AplombTech.WMS.JsonParser.DeviceMessages;
using AplombTech.WMS.JsonParser.DeviceMessages.Parsing;
using AplombTech.WMS.JsonParser.Entity;
using AplombTech.WMS.JsonParser.Topics;
using AplombTech.WMS.JsonParser.Topics.Classification;
using AplombTech.WMS.Messages.Commands;
using AplombTech.WMS.SensorDataLogBoundedContext.Repositories;
using AplombTech.WMS.SensorDataLogBoundedContext.UnitOfWorks;
using AplombTech.WMS.Persistence.UnitOfWorks;
using AplombTech.WMS.Domain.SummaryData;
using AplombTech.WMS.Domain.Vfds;
using AplombTech.WMS.Persistence.Facade;
using NServiceBus;
using AplombTech.WMS.Persistence.Repositories;
using AplombTech.WMS.Domain.Areas;

namespace AplombTech.WMS.MQTT.Data.Processor
{
    public class DataProcessor
    {
        private readonly ITopicClassifier _topicClassifier;
        private readonly IMessageParserFactory _messageParserFactory;
        private readonly IBus _bus;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataProcessor(IBus bus)
        {
            _topicClassifier = new TopicClassifier();
            _messageParserFactory = new MessageParserFactory();
            _bus = bus;
        }

        public void ProcessData(string topic, string message)
        {
            DataLog dataLog = LogSensorData(topic, message);

            //if (dataLog != null)
            //{
            //if (dataLog.ProcessingStatus == DataLog.ProcessingStatusEnum.None)
            //{
            try
            {
                var topicType = _topicClassifier.GetTopicType(topic);
                var messageParser = _messageParserFactory.CreateMessageParser(topicType);
                var parsedMessage = messageParser.ParseMessage(message);
                parsedMessage.LoggedAt = dataLog.LoggedAtSensor;
                parsedMessage.PumpStationId = dataLog.PumpStationId;
                {
                    switch (topicType)
                    {
                        case TopicType.SensorData:
                            var deviceDataMessage = (SensorMessage)parsedMessage;
                            try
                            {
                                ProcessMotorData(deviceDataMessage);
                            }
                            catch (Exception ex)
                            {
                                log.Info("Error Occured in ProcessMotorData method. Error: " + ex.ToString());
                            }

                            try
                            {
                                ProcessSensorData(deviceDataMessage);
                            }
                            catch (Exception ex)
                            {
                                log.Info("Error Occured in ProcessSensorData method. Error: " + ex.ToString());
                            }
                            break;
                        case TopicType.VfdData:
                            var vfdDataMessage = (VfdMessage)parsedMessage;
                            try
                            {
                                ProcessVfdData(vfdDataMessage);
                            }
                            catch (Exception ex)
                            {
                                log.Info("Error Occured in ProcessVfdData method. Error: " + ex.ToString());
                            }
                            break;
                        case TopicType.Configuration:
                            var configDataMessage = (ConfigurationMessageEntity)parsedMessage;
                            ProcessConfiguration(configDataMessage);
                            break;
                        default:
                            throw new InvalidTopicException();
                    }
                    //UpdateDataLog(dataLog.SensorDataLogID, DataLog.ProcessingStatusEnum.Done, null);
                }
            }
            catch (Exception ex)
            {
                log.Info("Error Occured in ProcessMessage method. Error: " + ex.ToString());
                //UpdateDataLog(dataLog.SensorDataLogID, DataLog.ProcessingStatusEnum.Failed, "Error Occured in ProcessMessage method. Error: " + ex.ToString());
            }
            //}
            //}
        }



        private void ProcessConfiguration(ConfigurationMessageEntity configDataMessage)
        {
            using (WMSUnitOfWork uow = new WMSUnitOfWork())
            {
                uow.CurrentObjectContext.Database.CommandTimeout = 120;
                ConfigureProcessRepository repoConfig = new ConfigureProcessRepository(uow.CurrentObjectContext);
                repoConfig.StoreConfigurationData(configDataMessage);
                uow.CurrentObjectContext.SaveChanges();
            }
        }

        private DataLog LogSensorData(string topic, string message)
        {
            try
            {
                DateTime loggedAtTime = DateTime.MinValue;
                string pumpStationUuid = "";
                int pumpStationId = 0;

                if (topic.StartsWith("wasa/") && topic.EndsWith("/sensor_data"))
                    loggedAtTime = JsonManager.GetSensorLoggedAtTime(message);
                if (topic.StartsWith("wasa/") && topic.EndsWith("/vfd_data"))
                    loggedAtTime = JsonManager.GetSensorLoggedAtTime(message);
                if (topic.StartsWith("wasa/") && topic.EndsWith("/configuration"))
                    loggedAtTime = JsonManager.GetConfigurationLoggedAtTime(message);

                pumpStationUuid = JsonManager.GetPumpStationIDFromJson(message);

                using (WMSUnitOfWork wmsUow = new WMSUnitOfWork())
                {
                    ProcessRepository repoProcess = new ProcessRepository(wmsUow.CurrentObjectContext);
                    pumpStationId = repoProcess.GetPumpStationFromUuid(pumpStationUuid)?.AreaId ?? 0;
                }

                DataLog data = new DataLog();
                data.PumpStationId = pumpStationId;
                data.LoggedAtSensor = loggedAtTime;

                return data;
                //using (SDLUnitOfWork uow = new SDLUnitOfWork())
                //{
                //    SDLRepository repo = new SDLRepository(uow.CurrentObjectContext);
                //    DataLog sensorLogData = repo.GetDataLog(topic, loggedAtTime, pumpStationId);
                //    if (sensorLogData == null)
                //    {
                //        DataLog data = repo.CreateDataLog(topic, message, loggedAtTime, pumpStationId);
                //        uow.CurrentObjectContext.SaveChanges();
                //        return data;
                //    }
                //    return sensorLogData;
                //}
            }
            catch (Exception ex)
            {
                log.Info("Error Occured in LogSensorData method of DataProcessor Class. Error: " + ex.ToString());
                return null;
            }
        }
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
                DataLog data = repo.CreateDataLog(topic, message, loggedAtSensor, stationId);
                uow.CurrentObjectContext.SaveChanges();
                return data;
            }
        }
        private void UpdateDataLog(long sensorDataLogId, DataLog.ProcessingStatusEnum status, string remarks)
        {
            using (SDLUnitOfWork uow = new SDLUnitOfWork())
            {
                SDLRepository repo = new SDLRepository(uow.CurrentObjectContext);
                DataLog data = repo.UpdateDataLog(sensorDataLogId, status, remarks);
                uow.CurrentObjectContext.SaveChanges();
            }
        }
        private void ProcessMotorData(SensorMessage messageObject)
        {
            using (SDLUnitOfWork uow = new SDLUnitOfWork())
            {
                uow.CurrentObjectContext.Database.CommandTimeout = 120;
                SDLRepository repo = new SDLRepository(uow.CurrentObjectContext);
                using (WMSUnitOfWork wmsUow = new WMSUnitOfWork())
                {
                    wmsUow.CurrentObjectContext.Database.CommandTimeout = 120;
                    ProcessRepository repoProcess = new ProcessRepository(wmsUow.CurrentObjectContext);
                    
                    foreach (MotorValue data in messageObject.Motors)
                    {
                        Motor motor = repoProcess.GetMotorByUuid(data.MotorUid);
                        if (motor != null && motor.IsActive)
                        {
                            string lastMotorStatus = motor.MotorStatus;
                            MotorData motorData = repo.CreateNewMotorData(data, messageObject.LoggedAt, motor);
                            UpdateLastDataOfMotor(data, motorData.ProcessAt, motor);
                            PublishMessageForMotorSummaryGeneration(data, messageObject.LoggedAt, motor);
                            if (motor is PumpMotor && data.MotorStatus != lastMotorStatus)
                            {
                                PublishMotorAlertMessage(data, motor);
                            }
                            wmsUow.CurrentObjectContext.SaveChanges();
                            uow.CurrentObjectContext.SaveChanges();
                        }
                    }
                }
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
            motor.AuditFields.LastUpdatedBy = "Automated";
            motor.AuditFields.LastUpdatedDateTime = DateTime.Now;
        }
        private void PublishMessageForMotorSummaryGeneration(MotorValue data, DateTime loggedAt, Motor motor)
        {
            var cmd = new MotorSummaryGenerationMessage
            {
                Uid = data.MotorUid,
                MotorStatus = data.MotorStatus,
                DataLoggedAt = loggedAt,
                MessageDateTime = DateTime.Now
            };
            if (motor is PumpMotor)
            {
                _bus.Send(cmd);
            }
        }
        private void PublishMotorAlertMessage(MotorValue data, Motor motor)
        {
            var cmd = new MotorAlertMessage
            {
                MotorId = motor.MotorID,
                MotorName = GetMotorName(motor),
                MotorStatus = data.MotorStatus,
                PumpStationName = motor.PumpStation.Name,
                AlertMessageType = (int)AlertType.AlertTypeEnum.OnOff,
                MessageDateTime = DateTime.Now
            };
            _bus.Send(cmd);

        }
        private string GetMotorName(Motor motor)
        {
            string motorName = String.Empty;
            if (motor is PumpMotor)
            {
                motorName = "Pump Motor";
            }
            if (motor is ChlorineMotor)
            {
                motorName = "Chlorine Motor";
            }
            return motorName;
        }
        private void ProcessSensorData(SensorMessage messageObject)
        {
            using (SDLUnitOfWork uow = new SDLUnitOfWork())
            {
                uow.CurrentObjectContext.Database.CommandTimeout = 120;
                SDLRepository repo = new SDLRepository(uow.CurrentObjectContext);
                using (WMSUnitOfWork wmsUow = new WMSUnitOfWork())
                {
                    wmsUow.CurrentObjectContext.Database.CommandTimeout = 120;
                    ProcessRepository repoProcess = new ProcessRepository(wmsUow.CurrentObjectContext);
                    foreach (SensorValue data in messageObject.Sensors)
                    {
                        Sensor sensor = repoProcess.GetSensorByUuid(data.SensorUUID);
                        if (sensor != null && sensor.IsActive)
                        {
                            SensorData sensorData = repo.CreateNewSensorData(data, messageObject.LoggedAt, sensor);
                            if (sensor is EnergySensor)
                            {
                                //if (sensor.PumpStation.UUID == "2348d15b-dcc4-4399-9154-47a1d57f6b2f")
                                //{
                                //    var previousValue = sensor.CurrentValue;
                                //    var recivedValue = sensorData.Value;

                                //    double timeDifferenceInHour = ((sensorData.ProcessAt - sensor.LastDataReceived.Value).TotalSeconds) / 3600;
                                //    var currentEnergyValue = Convert.ToDouble(recivedValue - previousValue) / timeDifferenceInHour;

                                //    File.AppendAllText("D:\\EnergyData.txt", sensor.PumpStation.Name + " = " + currentEnergyValue.ToString() + "\n");

                                //} 
                            }
                            else
                            {
                                UpdateLastDataOfSensor(sensorData, sensorData.ProcessAt, sensor);
                            }
                            PublishSensorMessageForSummaryGeneration(data, messageObject.LoggedAt, sensor);
                            if (!IsAlertGeneratedForThisHour(sensor.SensorId, wmsUow))
                                PublishSensorAlertMessage(data.Value, sensor, wmsUow.CurrentObjectContext);

                            wmsUow.CurrentObjectContext.SaveChanges();
                            uow.CurrentObjectContext.SaveChanges();
                        }
                    }
                }
            }
        }

        private void ProcessVfdData(VfdMessage messageObject)
        {
            using (SDLUnitOfWork uow = new SDLUnitOfWork())
            {
                uow.CurrentObjectContext.Database.CommandTimeout = 120;
                SDLRepository repo = new SDLRepository(uow.CurrentObjectContext);
                using (WMSUnitOfWork wmsUow = new WMSUnitOfWork())
                {
                    wmsUow.CurrentObjectContext.Database.CommandTimeout = 120;
                    VfdDataProcessorRepository repoProcess = new VfdDataProcessorRepository(wmsUow.CurrentObjectContext);
                    foreach (VfdValue data in messageObject.VfdList)
                    {
                        VariableFrequencyDrive vfd = repoProcess.GetVfdByUuid(data.VfdUid);
                        if (vfd != null && vfd.IsActive)
                        {
                            if (IsVfdDataValid(data))
                            {
                                VfdData vfdData = CreateNewVfdData(data, vfd.VfdId, messageObject.LoggedAt);
                                repo.AddVfdData(vfdData);

                                UpdateLastDataOfVfd(vfdData, vfd);
                                wmsUow.CurrentObjectContext.SaveChanges();
                                uow.CurrentObjectContext.SaveChanges();
                            }
                        }
                    }
                }
            }
        }

        private void UpdateLastDataOfVfd(VfdData vfdData, VariableFrequencyDrive vfd)
        {
            vfd.Current = vfdData.Current;
            vfd.Frequency = vfdData.Frequency;
            vfd.Power = vfdData.Power;
            vfd.Voltage = vfdData.Voltage;
            vfd.Energy = vfdData.Energy;
            vfd.OperatingHour = vfdData.OperatingHour;
            vfd.RunningHour = vfdData.RunningHour;
            vfd.LastDataReceived = DateTime.Now;
            vfd.AuditFields.LastUpdatedDateTime = DateTime.Now;
        }
        private VfdData CreateNewVfdData(VfdValue data, int vfdId, DateTime loggedAt)
        {
            VfdData vfdData = new VfdData();
            vfdData.VfdId = vfdId;
            vfdData.Current = data.Current;
            vfdData.Energy = data.Energy;
            vfdData.Frequency = data.Frequency;
            vfdData.Power = data.Power;
            vfdData.Voltage = data.Voltage;
            vfdData.OperatingHour = data.OperatingHour;
            vfdData.RunningHour = data.RunningHour;
            vfdData.LoggedAt = loggedAt;
            vfdData.ProcessAt = DateTime.Now;
            return vfdData;
        }
        private bool IsVfdDataValid(VfdValue data)
        {
            if (data.Current < 0 || data.Voltage < 0 || data.Energy < 0 || data.Power < 0)
                return false;

            return true;
        }

        private void PublishSensorMessageForSummaryGeneration(SensorValue data, DateTime loggedAt, Sensor sensor)
        {
            var cmd = new SensorSummaryGenerationMessage
            {
                Uid = data.SensorUUID,
                Value = Convert.ToDecimal(data.Value),
                SecondaryValue = Convert.ToDecimal(data.SecondaryValue),
                DataLoggedAt = loggedAt,
                MessageDateTime = DateTime.Now
            };
            if (sensor is FlowSensor || sensor is EnergySensor)
            {
                if (cmd.Value >= 0)
                {
                    _bus.Send(cmd);
                }
            }
            else
            {
                _bus.Send(cmd);
            }
        }
        private bool IsAlertGeneratedForThisHour(int sensorId, WMSUnitOfWork wmsUow)
        {
            AlertConfigurationRepository repo = new AlertConfigurationRepository(wmsUow.CurrentObjectContext);
            var alertMessage = repo.GetAlertLog(sensorId, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour);

            return alertMessage != null;
        }
        private void PublishSensorAlertMessage(string dataValue, Sensor sensor, WMSDBContext context)
        {
            if (sensor is EnergySensor || sensor is ACPresenceDetector || sensor is BatteryVoltageDetector) return;

            string sensorName = GetSensorName(sensor);

            decimal value = 0;
            if (sensor.DataType == Sensor.Data_Type.Float)
                value = Convert.ToDecimal(dataValue);
            if (sensor.DataType == Sensor.Data_Type.Boolean)
            {
                if (dataValue != null && dataValue.Contains("."))
                {
                    value = Convert.ToDecimal(Convert.ToBoolean(Convert.ToDecimal(dataValue)));
                }
                else
                {
                    value = Convert.ToDecimal(Convert.ToBoolean(Convert.ToDecimal(dataValue)));
                }
            }

            if (value == 0)
            {
                if (sensor is ChlorinePresenceDetector)
                {
                    SendSensorAlertMessage(value, sensorName, (int)AlertType.AlertTypeEnum.OnOff, sensor, context);
                }
                else
                {
                    SendSensorAlertMessage(value, sensorName, (int)AlertType.AlertTypeEnum.DataMissing, sensor, context);
                }

                return;
            }

            if (!(sensor is ChlorinePresenceDetector))
            {
                //Get the minimum value
                decimal minimumValue = GetMinimumValue(sensor.SensorId, context);
                if (value < minimumValue)
                {
                    if (sensor.MinimumValue != minimumValue)
                    {
                        sensor.MinimumValue = minimumValue;
                    }

                    SendSensorAlertMessage(value, sensorName, (int)AlertType.AlertTypeEnum.UnderThreshold, sensor, context);
                }
            }
        }
        private string GetSensorName(Sensor sensor)
        {
            string sensorName = String.Empty;
            if (sensor is FlowSensor)
            {
                sensorName = "Flow Sensor";
            }
            if (sensor is LevelSensor)
            {
                sensorName = "Level Sensor";
            }
            if (sensor is PressureSensor)
            {
                sensorName = "Pressure Sensor";
            }
            if (sensor is ChlorinePresenceDetector)
            {
                sensorName = "Chlorination Sensor";
            }

            if (sensor is ACPresenceDetector)
            {
                sensorName = "AC Presence Detector";
            }

            if (sensor is BatteryVoltageDetector)
            {
                sensorName = "Battery Voltage Detector";
            }
            return sensorName;
        }
        private void SendSensorAlertMessage(decimal value, string sensorName, int allertMessageType, Sensor sensor, WMSDBContext context)
        {
            if (allertMessageType == (int)AlertType.AlertTypeEnum.UnderThreshold)
            {
                CreateUnderThresoldData(value, sensor, context);
            }

            var cmd = new SensorAlertMessage
            {
                SensorId = sensor.SensorId,
                SensorName = sensorName,
                MinimumValue = sensor.MinimumValue,
                Value = value,
                PumpStationName = sensor.PumpStation.Name,
                AlertMessageType = allertMessageType,
                MessageDateTime = DateTime.Now
            };
            _bus.Send(cmd);
        }
        public void CreateUnderThresoldData(decimal value, Sensor sensor, WMSDBContext context)
        {
            UnderThresoldData underThresold = new UnderThresoldData();
            underThresold.Sensor = sensor;
            underThresold.Value = value;
            underThresold.LoggedAt = DateTime.Now;
            underThresold.Unit = sensor.UnitName;
            context.UnderThresoldDatas.Add(underThresold);
        }
        private decimal GetMinimumValue(int sensorId, WMSDBContext context)
        {
            decimal minimumValue = 0;
            ProcessRepository repo = new ProcessRepository(context);
            Sensor sensor = repo.GetSensorId(sensorId);
            minimumValue = sensor.MinimumValue;

            return minimumValue;
        }
        private void UpdateLastDataOfSensor(SensorData sensorData, DateTime loggedAt, Sensor sensor)
        {
            if (sensor.LastDataReceived != null)
            {
                if (sensor.LastDataReceived < loggedAt)
                {
                    sensor.CurrentValue = sensorData.Value;
                    sensor.LastDataReceived = loggedAt;
                    if (sensor is FlowSensor)
                    {
                        ((FlowSensor)sensor).MeterCubePerHourValue = sensorData.Rate.Value;
                        ((FlowSensor)sensor).CumulativeValue = sensorData.Value;//dailySummary.ReceivedValue;
                        ((FlowSensor)sensor).LitrePerMinuteValue = sensorData.Rate.Value; //(dataValue - prevCumulativeValue)*(6);
                    }

                }
            }
            else
            {
                sensor.CurrentValue = sensorData.Value;
                sensor.LastDataReceived = loggedAt;
            }
        }
    }
}
