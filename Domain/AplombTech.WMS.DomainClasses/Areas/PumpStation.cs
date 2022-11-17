using AplombTech.WMS.Domain.Devices;
using AplombTech.WMS.Domain.Features;
using AplombTech.WMS.Domain.Repositories;
using AplombTech.WMS.Domain.Sensors;
using AplombTech.WMS.Domain.Shared;
using NakedObjects;
using NakedObjects.Menu;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.WMS.Domain.Motors;
using ChlorineMotor = AplombTech.WMS.Domain.Motors.ChlorineMotor;
using PumpMotor = AplombTech.WMS.Domain.Motors.PumpMotor;

namespace AplombTech.WMS.Domain.Areas
{
  public class PumpStation : Area
  {
    #region Injected Services
    public LoggedInUserInfoDomainRepository LoggedInUserInfoDomainRepository { set; protected get; }
    #endregion
    public override string Name { get; set; }
    [DisplayName("UUID")]
    [NakedObjectsInclude]
    public override string UUID { get; set; }
    public string DisablePropertyDefault()
    {
      IList<Feature> features = LoggedInUserInfoDomainRepository.GetFeatureListByLoginUser();

      Feature feature =
          features.Where(w => w.FeatureCode == (int)Feature.AreaFeatureEnums.EditPumpStation
          && w.FeatureType.FeatureTypeName == FeatureType.FeatureTypeEnums.Area.ToString()).FirstOrDefault();

      if (feature == null)
      {
        return "You do not have permission to Edit";
      }

      return null;
    }

    #region Validations
    public string ValidateName(string areaName)
    {
      var rb = new ReasonBuilder();

      PumpStation station = (from obj in Container.Instances<PumpStation>()
                             where obj.Name == areaName
                             select obj).FirstOrDefault();

      if (station != null)
      {
        if (this.AreaId != station.AreaId)
        {
          rb.AppendOnCondition(true, "Duplicate PumpStation Name");
        }
      }
      return rb.Reason;
    }
    #endregion

    #region Get Properties      
    [MemberOrder(70), NotMapped]
    [Eagerly(EagerlyAttribute.Do.Rendering)]
    [DisplayName("Sensors")]
    [TableView(true, "CurrentValue", "LastDataReceived")]
    public IList<Sensor> Sensors
    {
      get
      {
        IList<Sensor> sensors = (from sensor in Container.Instances<Sensor>()
                                 where sensor.PumpStation.AreaId == this.AreaId
                                 select sensor).ToList();
        return sensors;
      }
    }

    [MemberOrder(80), NotMapped]
    [Eagerly(EagerlyAttribute.Do.Rendering)]
    [DisplayName("Motors")]
    [TableView(true, "UUID", "MotorStatus", "Auto", "Controllable", "LastDataReceived")]
    public IList<Motor> Motors
    {
      get
      {
        IList<Motor> motors = (from pump in Container.Instances<Motor>()
                               where pump.PumpStation.AreaId == this.AreaId
                               //&& pump.IsRemoved == false
                               select pump).ToList();
        return motors;
      }
    }

    [MemberOrder(90), NotMapped]
    //[Eagerly(EagerlyAttribute.Do.Rendering)]
    [DisplayName("Camera")]
    [TableView(true, "UUID", "URL")]
    public IList<Camera> Cameras
    {
      get
      {
        IList<Camera> cameras = (from camera in Container.Instances<Camera>()
                                 where camera.PumpStation.AreaId == this.AreaId
                                 select camera).ToList();
        return cameras;
      }
    }
    [DisplayName("CholorineMotor"), NotMapped]
    [NakedObjectsIgnore]
    public ChlorineMotor ChlorineMotors
    {
      get
      {
        ChlorineMotor pumps = (from pump in Container.Instances<ChlorineMotor>()
                               where pump.PumpStation.AreaId == this.AreaId
                               && pump.IsRemoved == false
                               select pump).FirstOrDefault();
        return pumps;
      }
    }
    [MemberOrder(50), NotMapped]
    [DisplayName("Pump"), NakedObjectsIgnore]
    public PumpMotor PumpMotors
    {
      get
      {
        PumpMotor pumps = (from pump in Container.Instances<PumpMotor>()
                           where pump.PumpStation.AreaId == this.AreaId
                           select pump).FirstOrDefault();
        return pumps;
      }
    }
    #endregion

    #region AddMotor (Action)
    [DisplayName("Add Motor")]
    public void AddMotor(Motor.MotorType motorType, string uuid, bool auto, bool controllable)
    {
      switch (motorType)
      {
        case Motor.MotorType.PumpMotor:
          CreatePumpMotor(uuid, auto, controllable);
          break;
        case Motor.MotorType.ChlorineMotor:
          CreateChlorineMotor(uuid, auto, controllable);
          break;
      }
    }
    private void CreatePumpMotor(string uuid, bool auto, bool controllable)
    {
      PumpMotor motor = Container.NewTransientInstance<PumpMotor>();

      motor.UUID = uuid;
      motor.Auto = auto;
      motor.Controllable = controllable;
      motor.PumpStation = this;

      Container.Persist(ref motor);
    }
    private void CreateChlorineMotor(string uuid, bool auto, bool controllable)
    {
      ChlorineMotor motor = Container.NewTransientInstance<ChlorineMotor>();

      motor.UUID = uuid;
      motor.Auto = auto;
      motor.Controllable = controllable;
      motor.PumpStation = this;

      Container.Persist(ref motor);
    }
    public bool HideAddMotor()
    {
      IList<Feature> features = LoggedInUserInfoDomainRepository.GetFeatureListByLoginUser();

      Feature feature =
          features.Where(w => w.FeatureCode == (int)Feature.AreaFeatureEnums.AddMotor
          && w.FeatureType.FeatureTypeName == FeatureType.FeatureTypeEnums.Area.ToString()).FirstOrDefault();

      if (feature == null)
        return true;

      return false;
    }
    #endregion

    #region AddCamera (Action)

    [DisplayName("Add Camera")]
    public void AddCamera(string url, string uuid)
    {
      Camera camera = Container.NewTransientInstance<Camera>();
      camera.URL = url;
      camera.UUID = uuid;

      camera.PumpStation = this;

      Container.Persist(ref camera);
    }
    public bool HideAddCamera()
    {
      IList<Feature> features = LoggedInUserInfoDomainRepository.GetFeatureListByLoginUser();

      Feature feature =
          features.Where(w => w.FeatureCode == (int)Feature.AreaFeatureEnums.AddCamera
          && w.FeatureType.FeatureTypeName == FeatureType.FeatureTypeEnums.Area.ToString()).FirstOrDefault();

      if (feature == null)
        return true;

      return false;
    }
    #endregion

    #region AddSensor (Action)

    [DisplayName("Add Sensor")]
    public void AddSensor([Required]Sensor.TransmitterType sensorType, string uuid, decimal minValue, double maxValue, Sensor.Data_Type dataType, string model, string version, string unit)
    {
      switch (sensorType)
      {
        case Sensor.TransmitterType.CHLORINE_PRESENCE_DETECTOR:
          CreateChlorinationSensor(uuid, minValue, maxValue, dataType, model, version, unit);
          break;

        case Sensor.TransmitterType.ENERGY_TRANSMITTER:
          CreateEnergySensor(uuid, minValue, maxValue, dataType, model, version, unit);
          break;

        case Sensor.TransmitterType.FLOW_TRANSMITTER:
          CreateFlowSensor(uuid, minValue, maxValue, dataType, model, version, unit);
          break;

        case Sensor.TransmitterType.LEVEL_TRANSMITTER:
          CreateLevelSensor(uuid, minValue, maxValue, dataType, model, version, unit);
          break;

        case Sensor.TransmitterType.PRESSURE_TRANSMITTER:
          CreatePressureSensor(uuid, minValue, maxValue, dataType, model, version, unit);
          break;
        case Sensor.TransmitterType.AC_PRESENCE_DETECTOR:
          CreateAcPresenseDetector(uuid, minValue, maxValue, dataType, model, version, unit);
          break;

        case Sensor.TransmitterType.BATTERY_VOLTAGE_DETECTOR:
          CreateBatteryVoltageDetector(uuid, minValue, maxValue, dataType, model, version, unit);
          break;
      }
    }
    public bool HideAddSensor()
    {
      IList<Feature> features = LoggedInUserInfoDomainRepository.GetFeatureListByLoginUser();

      Feature feature =
          features.Where(w => w.FeatureCode == (int)Feature.AreaFeatureEnums.AddSensor
          && w.FeatureType.FeatureTypeName == FeatureType.FeatureTypeEnums.Area.ToString()).FirstOrDefault();

      if (feature == null)
        return true;

      return false;
    }
    private void CreateChlorinationSensor(string uuid, decimal minValue, double maxValue, Sensor.Data_Type dataType, string model, string version, string unit)
    {
      ChlorinePresenceDetector sensor = Container.NewTransientInstance<ChlorinePresenceDetector>();

      sensor.UUID = uuid;
      sensor.MinimumValue = minValue;
      sensor.MaximumValue = maxValue;
      sensor.CurrentValue = 0;
      sensor.DataType = dataType;
      sensor.Model = model;
      sensor.UnitName = unit;
      sensor.Version = version;
      sensor.PumpStation = this;

      Container.Persist(ref sensor);
    }
    private void CreateFlowSensor(string uuid, decimal minValue, double maxValue, Sensor.Data_Type dataType, string model, string version, string unit)
    {
      FlowSensor sensor = Container.NewTransientInstance<FlowSensor>();

      sensor.UUID = uuid;
      sensor.MinimumValue = minValue;
      sensor.MaximumValue = maxValue;
      sensor.CurrentValue = 0;
      sensor.DataType = dataType;
      sensor.Model = model;
      sensor.UnitName = unit;
      sensor.Version = version;
      sensor.PumpStation = this;

      Container.Persist(ref sensor);
    }
    private void CreateEnergySensor(string uuid, decimal minValue, double maxValue, Sensor.Data_Type dataType, string model, string version, string unit)
    {
      EnergySensor sensor = Container.NewTransientInstance<EnergySensor>();

      sensor.UUID = uuid;
      sensor.MinimumValue = minValue;
      sensor.MaximumValue = maxValue;
      sensor.CurrentValue = 0;
      sensor.DataType = dataType;
      sensor.Model = model;
      sensor.UnitName = unit;
      sensor.Version = version;
      sensor.PumpStation = this;
      sensor.PumpStation = this;

      Container.Persist(ref sensor);
    }
    private void CreateLevelSensor(string uuid, decimal minValue, double maxValue, Sensor.Data_Type dataType, string model, string version, string unit)
    {
      LevelSensor sensor = Container.NewTransientInstance<LevelSensor>();

      sensor.UUID = uuid;
      sensor.MinimumValue = minValue;
      sensor.MaximumValue = maxValue;
      sensor.CurrentValue = 0;
      sensor.DataType = dataType;
      sensor.Model = model;
      sensor.UnitName = unit;
      sensor.Version = version;
      sensor.PumpStation = this;

      Container.Persist(ref sensor);
    }
    private void CreateAcPresenseDetector(string uuid, decimal minValue, double maxValue, Sensor.Data_Type dataType, string model, string version, string unit)
    {
      ACPresenceDetector sensor = Container.NewTransientInstance<ACPresenceDetector>();

      sensor.UUID = uuid;
      sensor.MinimumValue = minValue;
      sensor.MaximumValue = maxValue;
      sensor.CurrentValue = 0;
      sensor.DataType = dataType;
      sensor.Model = model;
      sensor.UnitName = unit;
      sensor.Version = version;
      sensor.PumpStation = this;

      Container.Persist(ref sensor);
    }
    private void CreateBatteryVoltageDetector(string uuid, decimal minValue, double maxValue, Sensor.Data_Type dataType, string model, string version, string unit)
    {
      BatteryVoltageDetector sensor = Container.NewTransientInstance<BatteryVoltageDetector>();

      sensor.UUID = uuid;
      sensor.MinimumValue = minValue;
      sensor.MaximumValue = maxValue;
      sensor.CurrentValue = 0;
      sensor.DataType = dataType;
      sensor.Model = model;
      sensor.UnitName = unit;
      sensor.Version = version;
      sensor.PumpStation = this;

      Container.Persist(ref sensor);
    }
    private void CreatePressureSensor(string uuid, decimal minValue, double maxValue, Sensor.Data_Type dataType, string model, string version, string unit)
    {
      PressureSensor sensor = Container.NewTransientInstance<PressureSensor>();

      sensor.UUID = uuid;
      sensor.MinimumValue = minValue;
      sensor.MaximumValue = maxValue;
      sensor.CurrentValue = 0;
      sensor.DataType = dataType;
      sensor.Model = model;
      sensor.UnitName = unit;
      sensor.Version = version;
      sensor.PumpStation = this;

      Container.Persist(ref sensor);
    }


    #endregion

    #region SetAddress (Action)
    [DisplayName("Set Address")]
    public void SetAddress([Required]string street1, string street2, string zipCode, string zone, string city)
    {
      Address address = Container.NewTransientInstance<Address>();
      address.Street1 = street1;
      address.Street2 = street2;
      address.ZipCode = zipCode;
      address.Zone = zone;
      address.City = city;

      Container.Persist(ref address);
      this.Address = address;
    }
    public bool HideSetAddress()
    {
      IList<Feature> features = LoggedInUserInfoDomainRepository.GetFeatureListByLoginUser();

      Feature feature =
          features.Where(w => w.FeatureCode == (int)Feature.AreaFeatureEnums.SetPumpStationAddress
          && w.FeatureType.FeatureTypeName == FeatureType.FeatureTypeEnums.Area.ToString()).FirstOrDefault();

      if (feature == null)
        return true;

      if (this.Address != null)
        return true;

      return false;
    }
    #endregion

    #region Menu
    public static void Menu(IMenu menu)
    {
      var sub = menu.CreateSubMenu("Device");
      sub.AddAction("AddMotor");
      sub.AddAction("AddCamera");
      sub.AddAction("AddSensor");

      menu.AddAction("SetAddress");
    }
    #endregion

    public override Area Parent { get; set; }
    [PageSize(10)]
    public IQueryable<DMA> AutoCompleteParent([MinLength(3)] string name)
    {
      IQueryable<DMA> dmas = (from z in Container.Instances<DMA>()
                              where z.Name.Contains(name)
                              select z).OrderBy(o => o.Name);

      return dmas;
    }
  }
}
