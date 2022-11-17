using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.WMS.Domain.Areas;
using AplombTech.WMS.Domain.Sensors;
using AplombTech.WMS.Domain.Shared;
using AplombTech.WMS.Domain.SummaryData;
using NakedObjects;
using AplombTech.WMS.QueryModel.Shared;

namespace AplombTech.WMS.QueryModel.Reports
{
    public class UnderThresold : IViewModel
    {
        public IDomainObjectContainer Container { set; protected get; }  //Injected service
        public virtual IList<PumpStation> PumpStations { get; set; }
        [Required(ErrorMessage = "Pump Station is required")]
        public int SelectedPumpStationId { get; set; }
        [Required(ErrorMessage = "Sensor Type is required")]
        public Sensor.TransmitterType TransmeType { get; set; }
        [Required(ErrorMessage = "Report Type is required")]
        public ReportType ReportType { get; set; }
        public Month Month { get; set; }
        public int Year { get; set; }
        public int Week { get; set; }
        public int Day { get; set; }
        public int Hour { get; set; }
        public DateTime ToDateTime { get; set; }
        public DateTime FromDateTime { get; set; }
        public List<UnderThresoldData> UnderThresoldDatas { get; set; }
        public string Title()
        {
            var t = Container.NewTitleBuilder();

            string title = "Under Thresold";

            t.Append(title);

            return t.ToString();
        }

        public string[] DeriveKeys()
        {
            string[] ids = PumpStations.Select(s => s.AreaId.ToString()).ToArray();
            return ids;
        }

        public void PopulateUsingKeys(string[] keys)
        {
            IList<string> ids = keys.ToList();
            PumpStations = Container.Instances<PumpStation>().Where(w => ids.Contains(w.AreaId.ToString())).ToList();
        }

        public string Unit { get; set; }
    }
}
