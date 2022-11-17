using AplombTech.WMS.Domain.Sensors;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Domain.SummaryData
{
    [Table("SensorOnOffSummaryData")]
    public class SensorOnOffSummaryData : OnOffData
    {
        #region  Navigation Properties
        public virtual Sensor Sensor { get; set; }
        #endregion
    }
}
