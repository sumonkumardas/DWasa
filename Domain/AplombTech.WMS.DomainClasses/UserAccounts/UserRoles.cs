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
    [Table("AspNetUserRoles")]
    public class UserRoles
    {
        #region Primitive Properties
        [Key, Column(Order = 0)]
        [NakedObjectsIgnore]
        [ForeignKey("LoginUser")]
        public virtual string UserId { get; set; }
        [Key, Column(Order = 1)]
        [NakedObjectsIgnore]
        [ForeignKey("Role")]
        public virtual string RoleId { get; set; }
        #endregion

        #region Navigation Properties
        [MemberOrder(40), Disabled]
        public virtual LoginUser LoginUser { get; set; }
        [MemberOrder(50), Disabled]
        public virtual Role Role { get; set; }
        #endregion
    }
}
