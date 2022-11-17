using NakedObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.WMS.Domain.Areas;
using AplombTech.WMS.Domain.Features;
using AplombTech.WMS.Domain.Repositories;
using AplombTech.WMS.Domain.Shared;

namespace AplombTech.WMS.Domain.Vfds
{
	public class VariableFrequencyDrive
	{
		#region Injected Services
		public IDomainObjectContainer Container { set; protected get; }
		public LoggedInUserInfoDomainRepository LoggedInUserInfoDomainRepository { set; protected get; }
		#endregion

		#region Life Cycle Methods
		// This region should contain any of the 'life cycle' convention methods (such as
		// Created(), Persisted() etc) called by the framework at specified stages in the lifecycle.

		public virtual void Persisting ()
		{
			AuditFields.InsertedBy = Container.Principal.Identity.Name;
			AuditFields.InsertedDateTime = DateTime.Now;
			AuditFields.LastUpdatedBy = Container.Principal.Identity.Name;
			AuditFields.LastUpdatedDateTime = DateTime.Now;
			this.IsActive = true;
		}
		public virtual void Updating ()
		{
			if (Container.Principal.Identity.Name != String.Empty)
				AuditFields.LastUpdatedBy = Container.Principal.Identity.Name;
			AuditFields.LastUpdatedDateTime = DateTime.Now;
		}
		#endregion

		#region Primitive Properties
		[Key, NakedObjectsIgnore]
		public virtual int VfdId { get; set; }
		[MemberOrder(10),Required]
		[StringLength(50),Disabled]
		public virtual string UUID { get; set; }
		[MemberOrder(20),Optionally,Disabled]
		public virtual decimal Current { get; set; }
		[MemberOrder(30), Optionally, Disabled]
		public virtual decimal Energy { get; set; }
		[MemberOrder(40), Optionally, Disabled]
		public virtual decimal Frequency { get; set; }
		[MemberOrder(50), Optionally, Disabled]
		public virtual decimal Power { get; set; }
		[MemberOrder(60), Optionally, Disabled]
		public virtual decimal Voltage { get; set; }
		[MemberOrder(70), Optionally, Disabled]
		public virtual decimal OperatingHour { get; set; }
		[MemberOrder(80), Optionally, Disabled]
		public virtual decimal RunningHour { get; set; }
		[MemberOrder(90), Disabled]
		public virtual DateTime? LastDataReceived { get; set; }
		[MemberOrder(100), Required, Disabled]
		public virtual bool IsActive { get; set; }
		[Optionally]
		public virtual string Model { get; set; }
		[Optionally]
		public virtual string Version { get; set; }

		#endregion

		public string DisablePropertyDefault ()
		{
			return "You do not have permission to Edit";
			//IList<Feature> features = LoggedInUserInfoDomainRepository.GetFeatureListByLoginUser();

			//Feature feature =
			//		features.Where(w => w.FeatureCode == (int)Feature.AreaFeatureEnums.EditSensor
			//		&& w.FeatureType.FeatureTypeName == FeatureType.FeatureTypeEnums.Area.ToString()).FirstOrDefault();

			//if (feature == null)
			//{
			//	return "You do not have permission to Edit";
			//}

			//return null;
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

		public bool HideAuditFields ()
		{
			return true;
		}
        #endregion
        #endregion

        #region  Navigation Properties
        [MemberOrder(200)]
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
    }
}
