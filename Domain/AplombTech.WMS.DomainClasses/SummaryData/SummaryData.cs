using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Domain.SummaryData
{
    public class SummaryData
    {
        #region Primitive Properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual Int64 SummaryDataId { get; set; }
        public virtual DateTime DataDate { get; set; }
        public virtual DateTime ProcessAt { get; set; }
        //public virtual decimal SecondaryValue { get; set; }

        #endregion
    }
}
