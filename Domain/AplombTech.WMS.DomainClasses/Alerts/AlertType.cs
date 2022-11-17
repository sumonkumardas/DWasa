using AplombTech.WMS.Domain.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.WMS.Domain.Repositories;
using NakedObjects;

namespace AplombTech.WMS.Domain.Alerts
{
    [Bounded]
    public class AlertType
    {
        #region Injected Services
        public IDomainObjectContainer Container { set; protected get; }
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
        public virtual int AlertTypeId { get; set; }
        [Title]
        [MemberOrder(10)]
        [StringLength(50)]
        public virtual string AlertName { get; set; }
        [MemberOrder(20)]
        public virtual string AlertMessage { get; set; }

        public enum AlertTypeEnum
        {
            OnOff = 1,
            UnderThreshold = 2,
            DataMissing = 3
        }
        #endregion

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

        #region Collection Properties
        private ICollection<AlertRecipient> _alertrecipients = new List<AlertRecipient>();
        [MemberOrder(50), NakedObjectsIgnore]
        //[Eagerly(EagerlyAttribute.Do.Rendering)]
        //[TableView(false, "AlertName")]
        public virtual ICollection<AlertRecipient> AlertRecipients
        {
            get
            {
                return _alertrecipients;
            }
            set
            {
                _alertrecipients = value;
            }
        }
        #endregion
    }
}
