using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.WMS.Domain.Sensors;

namespace AplombTech.WMS.Domain.SummaryData
{
    public class PressureSensorLastThirtyData
    {
        #region Primitive Properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual Int64 DataEntryId { get; set; }
        public virtual DateTime DataDate { get; set; }
        public virtual DateTime ProcessAt { get; set; }
        public virtual decimal Value { get; set; }

        #endregion

        #region  Navigation Properties
        public virtual Sensor Sensor { get; set; }
        #endregion
    }
}
