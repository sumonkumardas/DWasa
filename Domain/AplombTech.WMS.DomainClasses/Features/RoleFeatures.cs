using AplombTech.WMS.Domain.UserAccounts;
using NakedObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Domain.Features
{
    public class RoleFeatures
    {
        #region Primitive Properties
        [Key, Column(Order = 0)]
        [NakedObjectsIgnore]
        [ForeignKey("Feature")]
        public virtual int FeatureId { get; set; }
        [Key, Column(Order = 1)]
        [NakedObjectsIgnore]
        [ForeignKey("Role")]
        public virtual string RoleId { get; set; }
        #endregion

        #region Navigation Properties
        [MemberOrder(40), Disabled]
        public virtual Feature Feature { get; set; }
        [MemberOrder(50), Disabled]
        public virtual Role Role { get; set; }
        #endregion
    }
}
