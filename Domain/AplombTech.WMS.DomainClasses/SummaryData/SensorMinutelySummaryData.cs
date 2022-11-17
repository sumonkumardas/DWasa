using AplombTech.WMS.Domain.Sensors;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Domain.SummaryData
{
    [Table("SensorMinutelySummaryData")]
    public class SensorMinutelySummaryData : SummaryData
    {
        #region Primitive Properties
        public virtual decimal ReceivedValue { get; set; }
        public virtual decimal DataValue { get; set; }
        #endregion

        #region  Navigation Properties
        public virtual Sensor Sensor { get; set; }
        #endregion
    }
}
