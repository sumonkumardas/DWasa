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
    public class UnderThresoldData
    {
        #region Primitive Properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual Int64 UnderThresoldDataId { get; set; }
        public virtual string TransmeType { get; set; }
        public virtual decimal Value { get; set; }
        public virtual string Unit { get; set; }
        public virtual DateTime LoggedAt { get; set; }


        #endregion

        #region  Navigation Properties
        public virtual Sensor Sensor { get; set; }
        #endregion
    }
}
