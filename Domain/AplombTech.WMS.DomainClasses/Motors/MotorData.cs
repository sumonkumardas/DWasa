using NakedObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Domain.Motors
{
    public class MotorData
    {
        #region Primitive Properties
        [Key, NakedObjectsIgnore]
        public virtual Int64 MotorDataID { get; set; }
        [MemberOrder(10), Required]
        public virtual string MotorStatus { get; set; }
        public virtual bool Auto { get; set; }
        public virtual string LastCommand { get; set; }
        public virtual string LastCommandTime { get; set; }
        [MemberOrder(30), Required]
        public virtual DateTime LoggedAt { get; set; }
        [MemberOrder(30), Required]
        public virtual DateTime ProcessAt { get; set; }

        #endregion

        #region  Navigation Properties
        [MemberOrder(40), Required]
        public virtual int MotorId { get; set; }
        #endregion
    }
}
