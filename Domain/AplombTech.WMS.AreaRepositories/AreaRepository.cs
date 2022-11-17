using AplombTech.WMS.Domain.Areas;
using AplombTech.WMS.Domain.Features;
using AplombTech.WMS.Domain.Motors;
using AplombTech.WMS.Domain.Repositories;
using AplombTech.WMS.Domain.Sensors;
using NakedObjects;
using NakedObjects.Menu;
using NakedObjects.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.AreaRepositories
{
    [DisplayName("Area")]
    public class AreaRepository : AbstractFactoryAndRepository
    {
        #region Injected Services
        public LoggedInUserInfoDomainRepository LoggedInUserInfoDomainRepository { set; protected get; }
        #endregion
        public static void Menu(IMenu menu)
        {
            //menu.AddAction("FindCustomerByAccountNumber");
            menu.CreateSubMenu("Zone")
                .AddAction("CreateZone")
                .AddAction("FindZone")
                .AddAction("AllZones");
            menu.CreateSubMenu("DMA")
                .AddAction("FindDMA");
            menu.CreateSubMenu("PumpStation")
                .AddAction("FindPumpStation");
        }

        #region Zone
        //[AuthorizeAction(Roles = "AMSAdmin")]
        [DisplayName("Zone")]
        public Zone CreateZone([Required] string name)
        {
            Zone zone = Container.NewTransientInstance<Zone>();

            zone.Name = name;

            Container.Persist(ref zone);
            return zone;
        }

        #region Validations
        public string ValidateCreateZone(string name)
        {
            var rb = new ReasonBuilder();

            Zone zone = (from obj in Container.Instances<Zone>()
                         where obj.Name == name
                         select obj).FirstOrDefault();

            if (zone != null)
            {
                rb.AppendOnCondition(true, "Duplicate Zone Name");
            }
            return rb.Reason;
        }
        #endregion
        public bool HideCreateZone()
        {
            IList<Feature> features = LoggedInUserInfoDomainRepository.GetFeatureListByLoginUser();

            Feature feature =
                features.Where(w => w.FeatureCode == (int)Feature.AreaFeatureEnums.AddNewZone
                && w.FeatureType.FeatureTypeName == FeatureType.FeatureTypeEnums.Area.ToString()).FirstOrDefault();

            if (feature == null)
                return true;
            return false;
        }
        //[AuthorizeAction(Roles = "AMSAdmin")]
        [DisplayName("Find Zone")]
        [Eagerly(EagerlyAttribute.Do.Rendering)]
        [TableView(true, "Name")]
        public IQueryable<Zone> FindZone(string name)
        {
            IQueryable<Zone> zones = (from z in Container.Instances<Zone>()
                                      where z.Name.Contains(name)
                                      select z).OrderBy(o => o.Name);

            return zones;
        }
        public bool HideFindZone()
        {
            IList<Feature> features = LoggedInUserInfoDomainRepository.GetFeatureListByLoginUser();

            Feature feature =
                features.Where(w => w.FeatureCode == (int)Feature.AreaFeatureEnums.FindZone
                && w.FeatureType.FeatureTypeName == FeatureType.FeatureTypeEnums.Area.ToString()).FirstOrDefault();

            if (feature == null)
                return true;
            return false;
        }
        //[AuthorizeAction(Roles = "AMSAdmin")]
        [DisplayName("All Zones")]
        [Eagerly(EagerlyAttribute.Do.Rendering)]
        [TableView(true, "Name")]
        public IQueryable<Zone> AllZones()
        {
            return Container.Instances<Zone>();
        }
        public bool HideAllZones()
        {
            IList<Feature> features = LoggedInUserInfoDomainRepository.GetFeatureListByLoginUser();

            Feature feature =
                features.Where(w => w.FeatureCode == (int)Feature.AreaFeatureEnums.FindAllZones
                && w.FeatureType.FeatureTypeName == FeatureType.FeatureTypeEnums.Area.ToString()).FirstOrDefault();

            if (feature == null)
                return true;
            return false;
        }
        #endregion

        #region DMA
        //[AuthorizeAction(Roles = "AMSAdmin")]
        [DisplayName("Find DMA")]
        [Eagerly(EagerlyAttribute.Do.Rendering)]
        [TableView(true, "Name")]
        public IQueryable<DMA> FindDMA(string name)
        {
            IQueryable<DMA> dmas = (from z in Container.Instances<DMA>()
                                    where z.Name.Contains(name)
                                    select z).OrderBy(o => o.Name);

            return dmas;
        }
        public bool HideFindDMA()
        {
            IList<Feature> features = LoggedInUserInfoDomainRepository.GetFeatureListByLoginUser();

            Feature feature =
                features.Where(w => w.FeatureCode == (int)Feature.AreaFeatureEnums.FindDma
                && w.FeatureType.FeatureTypeName == FeatureType.FeatureTypeEnums.Area.ToString()).FirstOrDefault();

            if (feature == null)
                return true;
            return false;
        }
        #endregion

        #region PumpStation
        //[AuthorizeAction(Roles = "AMSAdmin")]
        [DisplayName("Find PumpStation")]
        [Eagerly(EagerlyAttribute.Do.Rendering)]
        [TableView(true, "Name")]
        public IQueryable<PumpStation> FindPumpStation(string name)
        {
            IQueryable<PumpStation> stations = (from z in Container.Instances<PumpStation>()
                                                where z.Name.Contains(name)
                                                select z).OrderBy(o => o.Name);

            return stations;
        }
        public bool HideFindPumpStation()
        {
            IList<Feature> features = LoggedInUserInfoDomainRepository.GetFeatureListByLoginUser();

            Feature feature =
                features.Where(w => w.FeatureCode == (int)Feature.AreaFeatureEnums.FindPumpStation
                && w.FeatureType.FeatureTypeName == FeatureType.FeatureTypeEnums.Area.ToString()).FirstOrDefault();

            if (feature == null)
                return true;
            return false;
        }
        #endregion
        public Sensor FindSensorByUuid(string uid)
        {
            Sensor sensor = Container.Instances<Sensor>().Where(w => w.UUID == uid).First();
            return sensor;
        }
        public Motor FindMotorByUuid(string uid)
        {
            Motor motor = Container.Instances<Motor>().Where(w => w.UUID == uid).First();
            return motor;
        }
        public void AddCamera(int pumpStationId, string uid, string url)
        {
            PumpStation pumpStation = Container.Instances<PumpStation>().Where(w => w.AreaId == pumpStationId).First();
            if (pumpStation.Cameras.Where(x => x.UUID == uid).Count() == 0)
            {
                pumpStation.AddCamera(url, uid);
            }
        }
        public void AddRouter(int pumpStationId, string uid, string ip, int port)
        {
            PumpStation pumpStation = Container.Instances<PumpStation>().Where(w => w.AreaId == pumpStationId).First();
            //if (pumpStation.Router == null)
            //{
            //    pumpStation.AddRouter(uid, ip, port);
            //}
        }
        public void AddPumpMotor(int pumpStationId, string uid, bool controllable, bool auto)
        {
            PumpStation pumpStation = Container.Instances<PumpStation>().Where(w => w.AreaId == pumpStationId).First();
            if (pumpStation.Motors == null || pumpStation.Motors.Count == 0)
            {
                pumpStation.AddMotor(Motor.MotorType.PumpMotor, uid, auto, controllable);
            }
        }

        public void AddCholorineMotor(int pumpStationId, string uid, bool controllable, bool auto)
        {
            PumpStation pumpStation = Container.Instances<PumpStation>().Where(w => w.AreaId == pumpStationId).First();
            if (pumpStation.Motors == null || pumpStation.Motors.Count == 0)
            {
                pumpStation.AddMotor(Motor.MotorType.ChlorineMotor, uid, auto, controllable);
            }
        }

    public void AddMotor(Motor.MotorType motortype, int pumpStationId, string uid, bool controllable, bool auto)
    {
      PumpStation pumpStation = Container.Instances<PumpStation>().Where(w => w.AreaId == pumpStationId).First();
      if (SameTypeMotorExits(pumpStation.Motors, motortype))
      {
        pumpStation.AddMotor(motortype, uid, auto, controllable);
      }
    }

    public void AddSensor(int pumpStationId, string uid, decimal minValue, double maxValue, Sensor.TransmitterType type, Sensor.Data_Type dataType, string model, string version, string unit)
        {
            PumpStation pumpStation = Container.Instances<PumpStation>().Where(w => w.AreaId == pumpStationId).First();
            if (!SameTypeSensorExists(pumpStation.Sensors, type))
            {
                pumpStation.AddSensor(type, uid, minValue, maxValue, dataType, model, version, unit);
            }
        }
        private bool SameTypeMotorExits(IList<Motor> motors,Motor.MotorType motorType)
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
        private bool SameTypeSensorExists(IList<Sensor> sensors, Sensor.TransmitterType type)
        {
            foreach (var sensor in sensors)
            {
                if (sensor is Domain.Sensors.PressureSensor && type == Sensor.TransmitterType.PRESSURE_TRANSMITTER)
                    return true;

                if (sensor is Domain.Sensors.FlowSensor && type == Sensor.TransmitterType.FLOW_TRANSMITTER)
                    return true;

                if (sensor is Domain.Sensors.EnergySensor && type == Sensor.TransmitterType.ENERGY_TRANSMITTER)
                    return true;

                if (sensor is Domain.Sensors.LevelSensor && type == Sensor.TransmitterType.LEVEL_TRANSMITTER)
                    return true;
            }

            return false;
        }
    }
}
