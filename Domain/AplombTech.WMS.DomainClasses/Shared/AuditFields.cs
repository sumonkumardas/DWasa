using NakedObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Domain.Shared
{
    [ComplexType]
    public class AuditFields
    {
        #region InsertedBy (String)
        [MemberOrder(130)]
        [NakedObjectsIgnore, Required]
        [Column("InsertedBy")]
        [StringLength(50)]
        public virtual string InsertedBy { get; set; }

        #endregion
        #region InsertedDateTime (DateTime)
        [MemberOrder(140), Mask("g")]
        [NakedObjectsIgnore, Required]
        [Column("InsertedDate")]
        public virtual DateTime InsertedDateTime { get; set; }

        #endregion
        #region LastUpdatedBy (String)
        [MemberOrder(150)]
        [NakedObjectsIgnore, Required]
        [Column("LastUpdatedBy")]
        [StringLength(50)]
        public virtual string LastUpdatedBy { get; set; }

        #endregion
        #region LastUpdatedDateTime (DateTime)
        [MemberOrder(160), Mask("g")]
        [NakedObjectsIgnore, Required]
        [Column("LastUpdatedDate")]
        public virtual DateTime LastUpdatedDateTime { get; set; }

        #endregion
    }
}
