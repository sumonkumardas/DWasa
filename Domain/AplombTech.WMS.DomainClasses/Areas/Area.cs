using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NakedObjects;
using AplombTech.WMS.Domain.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AplombTech.WMS.Domain.Repositories;

namespace AplombTech.WMS.Domain.Areas
{
    public class Area
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
        public virtual int AreaId { get; set; }
        [MemberOrder(5)]
        [StringLength(250)]
        public virtual string UUID { get; set; }
        [Title]
        [MemberOrder(10)]
        [StringLength(50)]
        public virtual string Name { get; set; }
        [NakedObjectsIgnore]
        public virtual string Location { get; set; }
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

        #region  Navigation Properties
        [MemberOrder(30)]
        public virtual Area Parent { get; set; }
        public bool HideParent()
        {
            if (this.Parent != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        [MemberOrder(40), Disabled]
        public virtual Address Address { get; set; }
        #endregion
    }
}
