using AplombTech.WMS.Domain.Shared;
using NakedObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Domain.Features
{
    public class Feature
    {
        #region Primitive Properties
        [Key, NakedObjectsIgnore]
        public virtual int FeatureId { get; set; }
        [Title, Required]
        [MemberOrder(20)]
        [StringLength(100)]
        public virtual string FeatureName { get; set; }
        [Required]
        [MemberOrder(30)]
        public virtual int FeatureCode { get; set; }
        #endregion

        #region Navigation Properties
        [MemberOrder(10), Required, Disabled]
        public virtual FeatureType FeatureType { get; set; }
        #endregion

        #region FeatureEnums
        public enum AreaFeatureEnums
        {
            AddNewZone = 1,
            EditZone = 2,
            SetZoneAddress = 3,
            FindZone = 4,
            FindAllZones = 5,
            AddDma = 6,
            EditDma = 7,
            SetDmaAddress = 8,
            FindDma = 9,
            AddPumpStation = 10,
            EditPumpStation = 11,
            SetPumpStationAddress = 12,
            FindPumpStation = 13,
            AddMotor = 14,
            EditMotor = 15,
            AddCamera = 16,
            EditCamera = 17,
            AddRouter = 18,
            EditRouter = 19,
            AddSensor = 20,
            EditSensor = 21,
            SensorActiveInactive = 22
        }
        public enum AlertFeatureEnums
        {
            AddAlertType = 1,
            EditAlertType = 2,
            AddDesignation = 3,
            EditDesignation = 4,
            AddAlertRecipient = 5,
            EditAlertRecipient = 6,
            ShowAlertRecipients = 7
        }
        public enum UserAccountsFeatureEnums
        {
            AddLoginUser = 1,
            EditLoginUser = 2,
            AddRole = 3,
            EditRole = 4,
            AssignRoleToUser = 5,
            AssignFeatureToRole = 6,
            ShowAllRoles = 7,
            ShowAllUsers = 8,
            AddFeatureType = 9,
            ShowAllFeatureTypes = 10,
            AddFeature = 11
        }
        public enum ReportFeatureEnums
        {
            GoogleMap = 1,
            ScadaMap = 2,
            DrillDown = 3,
            SummaryReport = 4,
            UnderThreshold = 5,
            CustomReport = 6,
            MotorOnOff=7
    }

        #endregion       
    }
}
