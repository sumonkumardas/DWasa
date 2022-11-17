using AplombTech.WMS.Domain.Features;
using AplombTech.WMS.Domain.Repositories;
using AplombTech.WMS.Domain.Shared;
using NakedObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Domain.Alerts
{
    public class AlertRecipient
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
        }
        public virtual void Updating()
        {
            AuditFields.LastUpdatedBy = Container.Principal.Identity.Name;
            AuditFields.LastUpdatedDateTime = DateTime.Now;
        }
        #endregion

        #region Primitive Properties
        [Key, NakedObjectsIgnore]
        public virtual int ReceipientId { get; set; }
        [Title]
        [MemberOrder(10)]
        [StringLength(100)]
        public virtual string Name { get; set; }
        [MemberOrder(30), Required]
        [Description("Example: +8801523456789")]
        [RegEx(Validation = @"^(?:\+88|01)?\d{11}\r?$", Message = "Not a valid mobile no")]
        public virtual string MobileNo { get; set; }
        [MemberOrder(40), Optionally]
        [RegEx(Validation = @"^[\-\w\.]+@[\-\w\.]+\.[A-Za-z]+$", Message = "Not a valid email address")]
        //\b[A - Z0 - 9._ % +-]+@[A-Z0-9.-]+\.[A-Z]{2,}\b
        public virtual string Email { get; set; }
        #endregion

        public string DisablePropertyDefault()
        {
            IList<Feature> features = LoggedInUserInfoDomainRepository.GetFeatureListByLoginUser();

            Feature feature =
                features.Where(w => w.FeatureCode == (int)Feature.AlertFeatureEnums.EditAlertRecipient
                && w.FeatureType.FeatureTypeName == FeatureType.FeatureTypeEnums.Alert.ToString()).FirstOrDefault();

            if (feature == null)
            {
                return "You do not have permission to Edit";
            }

            return null;
        }

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
        [MemberOrder(20)]
        public virtual Designation Designation { get; set; }
        #endregion

        #region Collection Properties
        private ICollection<AlertType> _alertTypes = new List<AlertType>();
        [MemberOrder(50), Disabled]
        [Eagerly(EagerlyAttribute.Do.Rendering)]
        [TableView(false, "AlertName")]
        public virtual ICollection<AlertType> AlertTypes
        {
            get
            {
                return _alertTypes;
            }
            set
            {
                _alertTypes = value;
            }
        }

        public virtual void AddToAlertTypes(AlertType value)
        {
            if (!(_alertTypes.Contains(value)))
            {
                _alertTypes.Add(value);
            }
        }

        public virtual void RemoveFromAlertTypes(AlertType value)
        {
            if (_alertTypes.Contains(value))
            {
                _alertTypes.Remove(value);
            }
        }

        public IList<AlertType> ChoicesRemoveFromAlertTypes(AlertType value)
        {
            return AlertTypes.ToList();
        }

        #endregion       
    }
}
