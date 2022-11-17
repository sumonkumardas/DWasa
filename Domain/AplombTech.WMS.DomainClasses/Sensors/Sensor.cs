using AplombTech.WMS.Domain.Areas;
using AplombTech.WMS.Domain.Features;
using AplombTech.WMS.Domain.Repositories;
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

namespace AplombTech.WMS.Domain.Sensors
{
    public class Sensor
    {
        #region Injected Services
        public IDomainObjectContainer Container { set; protected get; }
        public LoggedInUserInfoDomainRepository LoggedInUserInfoDomainRepository { set; protected get; }
        #endregion

        #region Life Cycle Methods
        // This region should contain any of the 'life cycle' convention methods (such as
        // Created(), Persisted() etc) called by the framework at specified stages in the lifecycle.

        public virtual void Persisting()
        {
            AuditFields.InsertedBy = Container.Principal.Identity.Name;
            AuditFields.InsertedDateTime = DateTime.Now;
            AuditFields.LastUpdatedBy = Container.Principal.Identity.Name;
            AuditFields.LastUpdatedDateTime = DateTime.Now;
            this.CurrentValue = 0;
            this.IsActive = true;
        }
        public virtual void Updating()
        {
            if(Container.Principal.Identity.Name != String.Empty)
                AuditFields.LastUpdatedBy = Container.Principal.Identity.Name;
            AuditFields.LastUpdatedDateTime = DateTime.Now;
        }
        #endregion

        #region Primitive Properties
        [Key, NakedObjectsIgnore]
        public virtual int SensorId { get; set; }
        [MemberOrder(10)]
        [StringLength(250)]
        public virtual string UUID { get; set; }
        [MemberOrder(40)]
        public virtual decimal MinimumValue { get; set; }
        [MemberOrder(50)]
        public virtual double MaximumValue { get; set; }
        [MemberOrder(20), Required, Disabled]
        public virtual decimal CurrentValue { get; set; }
        [MemberOrder(60), Disabled]
        public virtual DateTime? LastDataReceived { get; set; }
        [MemberOrder(70), Required, Disabled]
        public virtual bool IsActive { get; set; }
        public virtual Data_Type DataType { get; set; }
        public virtual string UnitName { get; set; }
        public virtual string Model { get; set; }
        public virtual string Version { get; set; }
        public virtual string Name { get; set; }

        //[DisplayName("SensorType"), MemberOrder(10), Required]
        //public virtual TransmitterType SensorType { get; set; }

        public enum TransmitterType
        {
            PRESSURE_TRANSMITTER = 1,
            CHLORINE_PRESENCE_DETECTOR = 2,
            ENERGY_TRANSMITTER = 3,
            FLOW_TRANSMITTER = 4,
            LEVEL_TRANSMITTER = 5,
            AC_PRESENCE_DETECTOR=6,
            BATTERY_VOLTAGE_DETECTOR =7,
            OTHER=8
        }

        public enum Data_Type
        {
            Float = 1,
            Boolean = 2
        }
        #endregion

        public string DisablePropertyDefault()
        {
            IList<Feature> features = LoggedInUserInfoDomainRepository.GetFeatureListByLoginUser();

            Feature feature =
                features.Where(w => w.FeatureCode == (int)Feature.AreaFeatureEnums.EditSensor
                && w.FeatureType.FeatureTypeName == FeatureType.FeatureTypeEnums.Area.ToString()).FirstOrDefault();

            if (feature == null)
            {
                return "You do not have permission to Edit";
            }

            return null;
        }
        //#region Get Properties
        //[MemberOrder(40), NotMapped]
        //[DisplayName("Current Value")]
        //public decimal GetCurrentValue
        //{
        //    get
        //    {
        //        Decimal value = 0;

        //        SensorData sensordata = (from data in Container.Instances<SensorData>()
        //                                 where data.Sensor.SensorID == this.SensorID
        //                                 select data).OrderByDescending(o => o.LoggedAt).FirstOrDefault();
        //        if(sensordata != null)
        //        {
        //            value = sensordata.Value;
        //        }
        //        return value;
        //    }
        //}
        //#endregion

        #region Complex Properties
        #region AuditFields (AuditFields)

        private AuditFields _auditFields = new AuditFields();

        [MemberOrder(250)]
        [Required]
        public virtual AuditFields AuditFields
        {
            get
            {
                return _auditFields;
            }
            set
            {
                _auditFields = value;
            }
        }

        public bool HideAuditFields()
        {
            return true;
        }
        #endregion
        #endregion

        #region  Navigation Properties
        [MemberOrder(100)]
        public virtual PumpStation PumpStation { get; set; }

        [PageSize(10)]
        public IQueryable<PumpStation> AutoCompletePumpStation([MinLength(3)] string name)
        {
            IQueryable<PumpStation> stations = (from z in Container.Instances<PumpStation>()
                                                where z.Name.Contains(name)
                                                select z).OrderBy(o => o.Name);

            return stations;
        }

        #endregion

        //private string GetSensorType()
        //{
        //    string type = String.Empty;

        //    switch (this.SensorType)
        //    {
        //        case TransmitterType.CHLORINE_TRANSMITTER:
        //            type = "CT";
        //            break;

        //        case TransmitterType.ENERGY_TRANSMITTER:
        //            type = "ET";
        //            break;

        //        case TransmitterType.FLOW_TRANSMITTER:
        //            type = "FT";
        //            break;

        //        case TransmitterType.LEVEL_TRANSMITTER:
        //            type = "LT";
        //            break;

        //        case TransmitterType.PRESSURE_TRANSMITTER:
        //            type = "PT";
        //            break;
        //    }
        //    return type;
        //}

        #region Behavior

        public void MakeActive(string confirmation)
        {
            if (confirmation == "I want to make Active")
                this.IsActive = true;
        }
        public IList<string> Choices0MakeActive()
        {
            IList<string> messages = new List<string>();
            messages.Add("I want to make Active");
            messages.Add("I do not want to make Active");

            return messages;
        }
        public bool HideMakeActive()
        {
            IList<Feature> features = LoggedInUserInfoDomainRepository.GetFeatureListByLoginUser();

            Feature feature =
                features.Where(w => w.FeatureCode == (int)Feature.AreaFeatureEnums.SensorActiveInactive
                && w.FeatureType.FeatureTypeName == FeatureType.FeatureTypeEnums.Area.ToString()).FirstOrDefault();

            if (feature == null)
                return true;

            if (this.IsActive)
                return true;

            return false;
        }
        public void MakeInActive(String confirmation)
        {
            if (confirmation == "I want to make InActive")
                this.IsActive = false;
        }
        public IList<string> Choices0MakeInActive()
        {
            IList<string> messages = new List<string>();
            messages.Add("I want to make InActive");
            messages.Add("I do not want to make InActive");

            return messages;
        }
        public bool HideMakeInActive()
        {
            IList<Feature> features = LoggedInUserInfoDomainRepository.GetFeatureListByLoginUser();

            Feature feature =
                features.Where(w => w.FeatureCode == (int)Feature.AreaFeatureEnums.SensorActiveInactive
                && w.FeatureType.FeatureTypeName == FeatureType.FeatureTypeEnums.Area.ToString()).FirstOrDefault();

            if (feature == null)
                return true;

            if (!this.IsActive)
                return true;

            return false;
        }
        #endregion

        #region Menu
        public static void Menu(IMenu menu)
        {
            menu.AddAction("MakeActive");
            menu.AddAction("MakeInActive");
        }
        #endregion
    }
}
