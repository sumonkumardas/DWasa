using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.QueryModel.Reports
{
    public class ReportSeries
    {
        public ReportSeries(decimal thresold)
        {
            data = new List<double>();
            threshold = thresold;
            negativeColor = "red";
        }

        public ReportSeries()
        {
            data = new List<double>();
            threshold = null;
            negativeColor = "transperant";
        }
        public string name { get; set; }
        public List<double> data { get; set; }
        public decimal? threshold { get; set; }
        public string negativeColor { get; set; }
    }
}
