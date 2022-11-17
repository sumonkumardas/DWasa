using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.WMS.Domain.Areas;
using AplombTech.WMS.Domain.Repositories;
using AplombTech.WMS.Domain.Shared;
using NakedObjects;

namespace AplombTech.WMS.Domain.Motors
{
    public class Motor
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

            this.IsActive = true;
        }
        public virtual void Updating()
        {
            AuditFields.LastUpdatedBy = Container.Principal.Identity.Name;
            AuditFields.LastUpdatedDateTime = DateTime.Now;
        }
        #endregion

        #region CONSTANT
        public const string OFF = "OFF";
        public const string ON = "ON";
        #endregion

        #region Primitive Properties
        [Key, NakedObjectsIgnore]
        public virtual int MotorID { get; set; }
        [MemberOrder(10),Required]
        [StringLength(250),Disabled]
        public virtual string UUID { get; set; }
        [MemberOrder(50), Disabled,Required]
        public virtual bool Auto { get; set; }
        [MemberOrder(60),Disabled, Required]
        public virtual bool Controllable { get; set; }
        [MemberOrder(65), Disabled]
        public virtual string MotorStatus { get; set; }
        public virtual string LastCommand { get; set; }
        public virtual string LastCommandTime { get; set; }
        [MemberOrder(70)]
        public virtual string RemoveRemarks { get; set; }

        public bool HideRemoveRemarks()
        {
            if (IsRemoved)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        [MemberOrder(80)]
        public virtual bool IsRemoved { get; set; }
        public bool HideIsRemoved()
        {
            return true;
        }
        [MemberOrder(90), Required, Disabled]
        public virtual bool IsActive { get; set; }
        [MemberOrder(100), Disabled]
        public virtual DateTime? LastDataReceived { get; set; }
        #endregion
        public enum MotorType
        {
            PumpMotor = 1,
            ChlorineMotor = 2
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
        [MemberOrder(110)]
        public virtual PumpStation PumpStation { get; set; }
        #endregion
    }
}
