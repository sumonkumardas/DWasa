using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Site.Models
{
    public class VariableFrequencyDriveViewModel
    {
        public virtual int VfdId { get; set; }
        public virtual decimal Current { get; set; }

        public virtual decimal Energy { get; set; }

        public virtual decimal Frequency { get; set; }

        public virtual decimal Power { get; set; }

        public virtual decimal Voltage { get; set; }

        public virtual decimal OperatingHour { get; set; }

        public virtual decimal RunningHour { get; set; }

    }
}
