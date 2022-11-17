using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.WMS.Domain.Areas;
using AplombTech.WMS.Domain.Sensors;
using AplombTech.WMS.QueryModel.Shared;
using NakedObjects;

namespace AplombTech.WMS.QueryModel.Reports
{
    [NotMapped]
    public class DrillDown: IViewModel
    {
        public string Title()
        {
            var t = Container.NewTitleBuilder();

            string title = "Drill Down";

            t.Append(title);

            return t.ToString();
        }

        public DrillDown()
        {
            Series = new List<ReportSeries>();
            SelectedSensor = new Sensor();
        }
        public IDomainObjectContainer Container { set; protected get; }  //Injected service
        public virtual IList<PumpStation> PumpStations { get; set; }
        [Required(ErrorMessage = "Pump Station is required")]
        public int SelectedPumpStationId { get; set; }
        public string[] DeriveKeys()
        {
            if (SelectedSensor != null)
            {
                string[] ids = new string[] { SelectedSensor.SensorId.ToString() };
                return ids;
            }
            return new string[] { };
        }
        [Required(ErrorMessage = "Sensor Type is required")]
        public Sensor.TransmitterType TransmeType { get; set; }

        public Sensor SelectedSensor { get; set; }
        [Required(ErrorMessage = "Report Type is required")]
        public ReportType ReportType { get; set; }
        public Month Month { get; set; }
        public int Year { get; set; }
        public int Week { get; set; }
        public int Day { get; set; }
        public int Hour { get; set; }
        public string GraphTitle { get; set; }
        public string GraphSubTitle { get; set; }
        public string[] XaxisCategory { get; set; }
        public DateTime FromDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public List<ReportSeries> Series { get; set; }
        public void PopulateUsingKeys(string[] keys)
        {
            if (keys.Length > 0)
            {
                int ids = Convert.ToInt32(keys[0]);
                SelectedSensor = Container.Instances<Sensor>().Where(w => w.SensorId == ids).FirstOrDefault();
            }
        }
        public string Unit { get; set; }
        public string Zone { get; set; }
        public string DMA { get; set; }
        public string PumpStation { get; set; }
        public string SensorName { get; set; }
    }
}
