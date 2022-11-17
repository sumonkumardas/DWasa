using AplombTech.WMS.Domain.Areas;
using AplombTech.WMS.Domain.Devices;
using AplombTech.WMS.Domain.Motors;
using AplombTech.WMS.Domain.Sensors;
using AplombTech.WMS.JsonParser.DeviceMessages;
using AplombTech.WMS.JsonParser.Entity;
using AplombTech.WMS.Persistence.Facade;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AplombTech.WMS.Persistence.Repositories
{
  public class ConfigureProcessRepository
  {
    private readonly WMSDBContext _wmsdbcontext;
    public ConfigureProcessRepository(WMSDBContext wmsdbcontext)
    {
      _wmsdbcontext = wmsdbcontext;
    }
    public void StoreConfigurationData(ConfigurationMessageEntity messageObject)
    {
      if (ValidateConfiguration(messageObject))
      {
        AddSensors(messageObject);
        AddMotors(messageObject);
        AddCameras(messageObject);
      }
      else
      {
        throw new ConfigureationValidationException();
      }
    }
    private bool ValidateConfiguration(ConfigurationMessageEntity messageEntity)
    {
      PumpStation pump = GetPumpStation(messageEntity.PumpStation.PumpStation_UuId);
      if (pump == null)
      {
        return false;
      }
      else
      {
        List<string> uuids = new List<string>();
        IList<Sensor> sensors = _wmsdbcontext.Sensors.Where(s => s.PumpStation.AreaId == pump.AreaId).ToList();
        IList<Motor> motors = _wmsdbcontext.Motors.Where(m => m.PumpStation.AreaId == pump.AreaId).ToList();
        List<Sensor.TransmitterType> distinctsensortypes = new List<Sensor.TransmitterType>();
        foreach (var sensor in messageEntity.Sensors)
        {
          Sensor.TransmitterType sensorTransmitterType = Enum.TryParse(sensor.SensorType, out sensorTransmitterType) ? sensorTransmitterType : Sensor.TransmitterType.AC_PRESENCE_DETECTOR;

          if (SameTypeSensorExists(sensors, sensorTransmitterType))
          {
            return false;
          }
          else
          {
            distinctsensortypes.Add(sensorTransmitterType);
            uuids.Add(sensor.Uuid);
          }
            
        }
        if (distinctsensortypes.Distinct().Count() != messageEntity.Sensors?.Count()) return false;
        foreach (var motor in messageEntity.Motors)
        {
          Motor.MotorType motorType = Enum.TryParse(motor.MotorType, out motorType) ? motorType : Motor.MotorType.ChlorineMotor;
          if (SameTypeMotorExits(motors, motorType))
          {
            return false;
          }
          else
            uuids.Add(motor.Uuid);
        }
        foreach (var camera in messageEntity.Cameras)
        {
          uuids.Add(camera.Uuid);
        }
        if (uuids.Count!=uuids.Distinct().Count())
        {
          return false;
        }
        if (isUuidsExits(uuids)) return false;
      }
      return true;
    }
    private bool isUuidsExits(List<string> uuids)
    {
      var isUuidExit = _wmsdbcontext.Cameras.Where(c => uuids.Contains(c.UUID)).Any();
      if (!isUuidExit)
      {
        isUuidExit = _wmsdbcontext.Sensors.Where(s => uuids.Contains(s.UUID)).Any();
        if (!isUuidExit)
        {
          var isMotorUuids = _wmsdbcontext.Motors.Where(m => uuids.Contains(m.UUID)).Any();
        }
      }
      return isUuidExit;
    }
    private PumpStation GetPumpStation(string pumpStationId)
    {
      var pump=(PumpStation)_wmsdbcontext.Areas.Where(p =>pumpStationId.Trim().Equals(p.UUID.Trim())).FirstOrDefault();
      return pump;
    }
    private void AddCameras(ConfigurationMessageEntity messageObject)
    {
      foreach (var camera in messageObject.Cameras)
      {
        AddCamera(messageObject.PumpStation.PumpStation_UuId, camera.Uuid, camera.Url);
      }
    }
    private void AddCamera(string pumpStationUuid, string uid, string url)
    {
      PumpStation pumpStation = (PumpStation)_wmsdbcontext.Areas.Where(w => w.UUID == pumpStationUuid).First();
      CreateCamera(pumpStation, url, uid);
    }
    private void CreateCamera(PumpStation pumpStation, string url, string uuid)
    {
      Camera camera =new Camera();
      camera.URL = url;
      camera.UUID = uuid;
      camera.AuditFields = new Domain.Shared.AuditFields() { InsertedBy = "auto", LastUpdatedBy = "auto", InsertedDateTime = DateTime.Now, LastUpdatedDateTime = DateTime.Now };

      camera.PumpStation = pumpStation;

      _wmsdbcontext.Cameras.Add(camera);
    }
    private void AddMotors(ConfigurationMessageEntity messageObject)
    {
      foreach (var motor in messageObject.Motors)
      {
        Motor.MotorType motorType = Enum.TryParse(motor.MotorType, out motorType) ? motorType : Motor.MotorType.ChlorineMotor;
        AddMotor(motorType, messageObject.PumpStation.PumpStation_UuId, motor.Uuid, motor.Controllable, motor.Auto);
      }
    }
    private void AddMotor(Motor.MotorType motortype, string pumpStationUuId, string uid, bool controllable, bool auto)
    {
      PumpStation pumpStation = (PumpStation) _wmsdbcontext.Areas.Where(w => w.UUID == pumpStationUuId).First();
      AddSpecialMotor(pumpStation, motortype, uid, auto, controllable);
    }
    private void AddSpecialMotor(PumpStation pumpStation,Motor.MotorType motorType, string uuid, bool auto, bool controllable)
    {
      switch (motorType)
      {
        case Motor.MotorType.PumpMotor:
          CreatePumpMotor(pumpStation,uuid, auto, controllable);
          break;
        case Motor.MotorType.ChlorineMotor:
          CreateChlorineMotor(pumpStation,uuid, auto, controllable);
          break;
      }
    }
    private void CreatePumpMotor(PumpStation pumpStation,string uuid, bool auto, bool controllable)
    {
      PumpMotor motor = new PumpMotor();

      motor.UUID = uuid;
      motor.Auto = auto;
      motor.Controllable = controllable;
      motor.IsActive = true;
      motor.PumpStation = pumpStation;
      motor.AuditFields = new Domain.Shared.AuditFields() { InsertedBy = "auto", LastUpdatedBy = "auto", InsertedDateTime = DateTime.Now, LastUpdatedDateTime = DateTime.Now };

      _wmsdbcontext.PumpMotors.Add(motor);
    }
    private void CreateChlorineMotor(PumpStation pumpStation, string uuid, bool auto, bool controllable)
    {
      ChlorineMotor motor = new ChlorineMotor();

      motor.UUID = uuid;
      motor.Auto = auto;
      motor.Controllable = controllable;
      motor.IsActive = true;
      motor.PumpStation = pumpStation;
      motor.AuditFields = new Domain.Shared.AuditFields() { InsertedBy = "auto", LastUpdatedBy = "auto", InsertedDateTime = DateTime.Now, LastUpdatedDateTime = DateTime.Now };

      _wmsdbcontext.ChlorineMotors.Add(motor);
    }
    private bool SameTypeMotorExits(IList<Motor> motors, Motor.MotorType motorType)
    {
      foreach (var motor in motors)
      {
        if (motor is PumpMotor && motorType == Motor.MotorType.PumpMotor)
        {
          return true;
        }
        if (motor is ChlorineMotor && motorType == Motor.MotorType.ChlorineMotor)
        {
          return true;
        }
      }
      return false;
    }
    private void AddSensors(ConfigurationMessageEntity messageObject)
    {
      foreach (var sensor in messageObject.Sensors)
      {
        Sensor.TransmitterType sensorTransmitterType = Enum.TryParse(sensor.SensorType, out sensorTransmitterType) ? sensorTransmitterType : Sensor.TransmitterType.AC_PRESENCE_DETECTOR;
        if (string.Equals(sensor.DataType, "Double"))
        {
          sensor.DataType = "Float";
        }
        Sensor.Data_Type sensorDataType = Enum.TryParse(sensor.DataType, out sensorDataType) ? sensorDataType : Sensor.Data_Type.Float;
        AddSensor(messageObject.PumpStation.PumpStation_UuId, sensor.Uuid, sensor.MinValue, sensor.MaxValue, sensorTransmitterType, sensorDataType, sensor.Model, sensor.Version, sensor.Unit);
      }
    }
    private void AddSensor(string pumpStationUuId, string uid, decimal minValue, double maxValue, Sensor.TransmitterType type, Sensor.Data_Type dataType, string model, string version, string unit)
    {
      PumpStation pumpStation = (PumpStation) _wmsdbcontext.Areas.Where(w => w.UUID == pumpStationUuId).First();
      AddSpecialSensor(pumpStation, type, uid, minValue, maxValue, dataType, model, version, unit);
    }
    private void AddSpecialSensor(PumpStation pumpStation,Sensor.TransmitterType sensorType, string uuid, decimal minValue, double maxValue, Sensor.Data_Type dataType, string model, string version, string unit)
    {
      switch (sensorType)
      {
        case Sensor.TransmitterType.CHLORINE_PRESENCE_DETECTOR:
          CreateChlorinationSensor(pumpStation,uuid, minValue, maxValue, dataType, model, version, unit);
          break;

        case Sensor.TransmitterType.ENERGY_TRANSMITTER:
          CreateEnergySensor(pumpStation, uuid, minValue, maxValue, dataType, model, version, unit);
          break;

        case Sensor.TransmitterType.FLOW_TRANSMITTER:
          CreateFlowSensor(pumpStation, uuid, minValue, maxValue, dataType, model, version, unit);
          break;

        case Sensor.TransmitterType.LEVEL_TRANSMITTER:
          CreateLevelSensor(pumpStation, uuid, minValue, maxValue, dataType, model, version, unit);
          break;

        case Sensor.TransmitterType.PRESSURE_TRANSMITTER:
          CreatePressureSensor(pumpStation, uuid, minValue, maxValue, dataType, model, version, unit);
          break;
        case Sensor.TransmitterType.AC_PRESENCE_DETECTOR:
          CreateAcPresenseDetector(pumpStation, uuid, minValue, maxValue, dataType, model, version, unit);
          break;

        case Sensor.TransmitterType.BATTERY_VOLTAGE_DETECTOR:
          CreateBatteryVoltageDetector(pumpStation, uuid, minValue, maxValue, dataType, model, version, unit);
          break;
        
      }
    }
    private void CreateChlorinationSensor(PumpStation pumpStation, string uuid, decimal minValue, double maxValue, Sensor.Data_Type dataType, string model, string version, string unit)
    {
      ChlorinePresenceDetector sensor = new ChlorinePresenceDetector();

      sensor.UUID = uuid;
      sensor.MinimumValue = minValue;
      sensor.MaximumValue = maxValue;
      sensor.CurrentValue = 0;
      sensor.DataType = dataType;
      sensor.Model = model;
      sensor.UnitName = unit;
      sensor.Version = version;
      sensor.PumpStation = pumpStation;
      sensor.IsActive = true;
      sensor.AuditFields = new Domain.Shared.AuditFields() { InsertedBy = "auto", LastUpdatedBy = "auto", InsertedDateTime = DateTime.Now, LastUpdatedDateTime = DateTime.Now };

      _wmsdbcontext.ChlorinePresenceDetectors.Add(sensor);
    }
    private void CreateFlowSensor(PumpStation pumpStation, string uuid, decimal minValue, double maxValue, Sensor.Data_Type dataType, string model, string version, string unit)
    {
      FlowSensor sensor = new FlowSensor();

      sensor.UUID = uuid;
      sensor.MinimumValue = minValue;
      sensor.MaximumValue = maxValue;
      sensor.CurrentValue = 0;
      sensor.DataType = dataType;
      sensor.Model = model;
      sensor.UnitName = unit;
      sensor.Version = version;
      sensor.IsActive = true;
      sensor.PumpStation = pumpStation;
      sensor.AuditFields = new Domain.Shared.AuditFields() { InsertedBy = "auto", LastUpdatedBy = "auto", InsertedDateTime = DateTime.Now, LastUpdatedDateTime = DateTime.Now };

      _wmsdbcontext.FlowSensors.Add(sensor);
    }
    private void CreateEnergySensor(PumpStation pumpStation, string uuid, decimal minValue, double maxValue, Sensor.Data_Type dataType, string model, string version, string unit)
    {
      EnergySensor sensor = new EnergySensor();

      sensor.UUID = uuid;
      sensor.MinimumValue = minValue;
      sensor.MaximumValue = maxValue;
      sensor.CurrentValue = 0;
      sensor.DataType = dataType;
      sensor.Model = model;
      sensor.UnitName = unit;
      sensor.Version = version;
      sensor.IsActive = true;
      sensor.PumpStation = pumpStation;
      sensor.AuditFields = new Domain.Shared.AuditFields() { InsertedBy = "auto", LastUpdatedBy = "auto", InsertedDateTime = DateTime.Now, LastUpdatedDateTime = DateTime.Now };

      _wmsdbcontext.EnergySensors.Add(sensor);
    }
    private void CreateLevelSensor(PumpStation pumpStation, string uuid, decimal minValue, double maxValue, Sensor.Data_Type dataType, string model, string version, string unit)
    {
      LevelSensor sensor = new LevelSensor();

      sensor.UUID = uuid;
      sensor.MinimumValue = minValue;
      sensor.MaximumValue = maxValue;
      sensor.CurrentValue = 0;
      sensor.DataType = dataType;
      sensor.Model = model;
      sensor.UnitName = unit;
      sensor.Version = version;
      sensor.IsActive = true;
      sensor.PumpStation = pumpStation;
      sensor.AuditFields = new Domain.Shared.AuditFields() { InsertedBy = "auto", LastUpdatedBy = "auto", InsertedDateTime = DateTime.Now, LastUpdatedDateTime = DateTime.Now };

      _wmsdbcontext.LevelSensors.Add(sensor);
    }
    private void CreateAcPresenseDetector(PumpStation pumpStation, string uuid, decimal minValue, double maxValue, Sensor.Data_Type dataType, string model, string version, string unit)
    {
      ACPresenceDetector sensor = new ACPresenceDetector();

      sensor.UUID = uuid;
      sensor.MinimumValue = minValue;
      sensor.MaximumValue = maxValue;
      sensor.CurrentValue = 0;
      sensor.DataType = dataType;
      sensor.Model = model;
      sensor.UnitName = unit;
      sensor.Version = version;
      sensor.IsActive = true;
      sensor.PumpStation = pumpStation;
      sensor.AuditFields = new Domain.Shared.AuditFields() { InsertedBy = "auto", LastUpdatedBy = "auto", InsertedDateTime = DateTime.Now, LastUpdatedDateTime = DateTime.Now };

      _wmsdbcontext.ACPresenceDetectors.Add(sensor);
    }
    private void CreateBatteryVoltageDetector(PumpStation pumpStation, string uuid, decimal minValue, double maxValue, Sensor.Data_Type dataType, string model, string version, string unit)
    {
      BatteryVoltageDetector sensor = new BatteryVoltageDetector();

      sensor.UUID = uuid;
      sensor.MinimumValue = minValue;
      sensor.MaximumValue = maxValue;
      sensor.CurrentValue = 0;
      sensor.DataType = dataType;
      sensor.Model = model;
      sensor.UnitName = unit;
      sensor.Version = version;
      sensor.IsActive = true;
      sensor.PumpStation = pumpStation;
      sensor.AuditFields = new Domain.Shared.AuditFields() { InsertedBy = "auto", LastUpdatedBy = "auto", InsertedDateTime = DateTime.Now, LastUpdatedDateTime = DateTime.Now };

      _wmsdbcontext.BatteryVoltageDetectors.Add(sensor);
    }
    private void CreatePressureSensor(PumpStation pumpStation, string uuid, decimal minValue, double maxValue, Sensor.Data_Type dataType, string model, string version, string unit)
    {
      PressureSensor sensor = new PressureSensor();

      sensor.UUID = uuid;
      sensor.MinimumValue = minValue;
      sensor.MaximumValue = maxValue;
      sensor.CurrentValue = 0;
      sensor.DataType = dataType;
      sensor.Model = model;
      sensor.UnitName = unit;
      sensor.Version = version;
      sensor.IsActive = true;
      sensor.PumpStation = pumpStation;
      sensor.AuditFields = new Domain.Shared.AuditFields() { InsertedBy = "auto", LastUpdatedBy = "auto", InsertedDateTime = DateTime.Now, LastUpdatedDateTime = DateTime.Now };

      _wmsdbcontext.PressureSensors.Add(sensor);
    }
    
    private bool SameTypeSensorExists(IList<Sensor> sensors, Sensor.TransmitterType type)
    {
      foreach (var sensor in sensors)
      {
        if (sensor is PressureSensor && type == Sensor.TransmitterType.PRESSURE_TRANSMITTER)
          return true;

        if (sensor is ChlorinePresenceDetector && type == Sensor.TransmitterType.CHLORINE_PRESENCE_DETECTOR)
          return true;

        if (sensor is EnergySensor && type == Sensor.TransmitterType.ENERGY_TRANSMITTER)
          return true;

        if (sensor is FlowSensor && type == Sensor.TransmitterType.FLOW_TRANSMITTER)
          return true;
        if (sensor is LevelSensor && type == Sensor.TransmitterType.LEVEL_TRANSMITTER)
          return true;
        if (sensor is ACPresenceDetector && type == Sensor.TransmitterType.AC_PRESENCE_DETECTOR)
          return true;
        if (sensor is BatteryVoltageDetector && type == Sensor.TransmitterType.BATTERY_VOLTAGE_DETECTOR)
          return true;
        
      }

      return false;
    }
    private Camera CreateCamera(CameraEntity entity)
    {
      Camera model = new Camera();
      model.UUID = entity.Uuid;
      model.URL = entity.Url;
      _wmsdbcontext.Cameras.Add(model);
      return null;
    }
    private Sensor CreateSensor(SensorEntity entity)
    {
      return null;
    }
    private Motor CreateMotor(MotorEntity entity)
    {
      return null;
    }
  }
}
