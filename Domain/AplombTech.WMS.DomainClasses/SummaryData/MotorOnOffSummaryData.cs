using AplombTech.WMS.Domain.Motors;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Domain.SummaryData
{
    [Table("MotorOnOffSummaryData")]
    public class MotorOnOffSummaryData : OnOffData
    {
        #region  Navigation Properties
        public virtual Motor Motor { get; set; }
        #endregion
    }
}
