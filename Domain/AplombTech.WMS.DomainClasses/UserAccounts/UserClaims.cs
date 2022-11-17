using NakedObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Domain.UserAccounts
{
    [Table("AspNetUserClaims")]
    public class UserClaims
    {
        #region Primitive Properties
        [Key, NakedObjectsIgnore]
        public virtual int Id { get; set; }
        [NakedObjectsIgnore]
        [ForeignKey("LoginUser"), Required]
        public virtual string UserId { get; set; }
        [Optionally]
        public virtual string ClaimType { get; set; }
        [Optionally]
        public virtual string ClaimValue { get; set; }
        #endregion   

        #region Navigation Properties
        [MemberOrder(40), Disabled]
        public virtual LoginUser LoginUser { get; set; }
        #endregion
    }
}
