using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NakedObjects;

namespace AplombTech.WMS.Domain.Vfds
{
	public class VfdData
	{
		#region Primitive Properties
		[Key, NakedObjectsIgnore]
		public virtual Int64 VfdDataID { get; set; }
		public virtual decimal Current { get; set; }
		[MemberOrder(30), Required, Disabled]
		public virtual decimal Energy { get; set; }
		[MemberOrder(40), Required, Disabled]
		public virtual decimal Frequency { get; set; }
		[MemberOrder(50), Required, Disabled]
		public virtual decimal Power { get; set; }
		[MemberOrder(60), Required, Disabled]
		public virtual decimal Voltage { get; set; }
		[MemberOrder(70), Required, Disabled]
		public virtual decimal OperatingHour { get; set; }
		[MemberOrder(80), Required, Disabled]
		public virtual decimal RunningHour { get; set; }
		[MemberOrder(30), Required]
		public virtual DateTime LoggedAt { get; set; }
		[MemberOrder(30), Required]
		public virtual DateTime ProcessAt { get; set; }

		#endregion

		#region  Navigation Properties
		[MemberOrder(40), Required]
		public virtual int VfdId { get; set; }
		#endregion
	}
}
