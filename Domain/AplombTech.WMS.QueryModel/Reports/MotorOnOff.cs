using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.WMS.Domain.Areas;
using AplombTech.WMS.Domain.Sensors;
using AplombTech.WMS.Domain.SummaryData;
using AplombTech.WMS.QueryModel.Shared;
using NakedObjects;

namespace AplombTech.WMS.QueryModel.Reports
{
  [NotMapped]
  public class MotorOnOff : IViewModel
  {
    public string Title()
    {
      var t = Container.NewTitleBuilder();

      string title = "Motor On Off";

      t.Append(title);

      return t.ToString();
    }

    public MotorOnOff()
    {
      Series = new List<ReportSeries>();
    }
    public IDomainObjectContainer Container { set; protected get; }  //Injected service
    public virtual IList<PumpStation> PumpStations { get; set; }
    [Required(ErrorMessage = "Pump Station is required")]
    public int SelectedPumpStationId { get; set; }
    public string[] DeriveKeys()
    {
      return new string[] { };
    }
    
    public string GraphTitle { get; set; }
    public string GraphSubTitle { get; set; }
    public string[] XaxisCategory { get; set; }
    public DateTime FromDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public List<ReportSeries> Series { get; set; }
    public List<MotorOnOffSummaryData> MotorOnOffDatas { get; set; }
    public void PopulateUsingKeys(string[] keys)
    {
      if (keys.Length > 0)
      {
        int ids = Convert.ToInt32(keys[0]);
        SelectedPumpStationId = Container.Instances<PumpStation>().Where(w => w.AreaId == ids).FirstOrDefault().AreaId;
      }
    }
    public string Unit { get; set; }
    public string PumpStation { get; set; }
  }
}
