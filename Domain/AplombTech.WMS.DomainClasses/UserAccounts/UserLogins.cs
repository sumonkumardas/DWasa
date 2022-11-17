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
    [Table("AspNetUserLogins")]
    public class UserLogins
    {
        #region Primitive Properties
        [Key, Column(Order = 0), NakedObjectsIgnore]
        public virtual string LoginProvider { get; set; }
        [Key, Column(Order = 1)]
        public virtual string ProviderKey { get; set; }
        [Key, Column(Order = 2)]
        [NakedObjectsIgnore]
        [ForeignKey("LoginUser")]
        public virtual string UserId { get; set; }
        #endregion   

        #region Navigation Properties
        [MemberOrder(40), Disabled]
        public virtual LoginUser LoginUser { get; set; }
        #endregion
    }
}
