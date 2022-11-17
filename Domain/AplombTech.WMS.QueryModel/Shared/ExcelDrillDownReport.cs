using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.QueryModel.Shared
{
    public class ExcelDrillDownReport
    {
        public string Sensor { get; set; }

        public string Value { get; set; }
        public string Unit { get; set; }
        public string Rate { get; set; }
        public string RateUnit { get; set; }
        public string Date { get; set; }
    }
}
