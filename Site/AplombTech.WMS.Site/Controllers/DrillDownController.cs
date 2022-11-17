using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using AplombTech.WMS.QueryModel.Reports;
using AplombTech.WMS.QueryModel.Repositories;
using AplombTech.WMS.Domain.Sensors;
using AplombTech.WMS.QueryModel.Shared;
using NakedObjects;
using NakedObjects.Facade;
using NakedObjects.Web.Mvc.Controllers;
using Newtonsoft.Json;

namespace AplombTech.WMS.Site.Controllers
{
    [Authorize]
    public class DrillDownController : SystemControllerImpl
    {
        #region Injected Services
        public ReportRepository _reportRepository { set; protected get; }
        #endregion

        public DrillDownController(IFrameworkFacade facade, IIdHelper idHelper) : base(facade, idHelper) { }

        // Uncomment this constructor if you wish to have an IDomainObjectContainer and/or domain services injected.
        // You will also need to ensure you have NakedObjects.Core package installed & add using NakedObjects;
        public DrillDownController(IFrameworkFacade facade, IIdHelper idHelper, INakedObjectsFramework nakedObjectsFramework)
            : base(facade, idHelper)
        {
            nakedObjectsFramework.DomainObjectInjector.InjectInto(this);
        }

        // GET: Report
        public ActionResult ShowDrillDownFromMap()
        {
            DrillDown model = _reportRepository.DrillDown();
            return View("~/Views/DrillDown/DrillDown.cshtml", model);
        }

        public JsonResult GetReportModel(DrillDown model)
        {
            model = _reportRepository.GetReportData(model);

            return Json(new { Data = model, IsSuccess = true }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ExportToExcel(DrillDown m)
        {
            m = _reportRepository.GetReportData(m);
            
            try
            {
                var grid = new System.Web.UI.WebControls.GridView();
                if (m.ReportType == ReportType.Daily || m.ReportType == ReportType.Weekly || m.ReportType == ReportType.Monthly)
                {
                    List<ExcelDrillDownReport> report = GetDatewiseReportData(m);

                    grid.DataSource = report;
                }
                else
                {
                    List<DrillDownReport> report = GetReportData(m);
                    grid.DataSource = report;
                }
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                //Report Header
                htw.WriteLine("<br>");
                htw.WriteLine("<b><font size='3'>Zone - " + m.Zone + "</font></b>");
                htw.WriteLine("<br>");
                htw.WriteLine("<b><font size='3'>DMA - " + m.DMA + "</font></b>");
                htw.WriteLine("<br>");
                htw.WriteLine("<b><font size='3'>Pump Station - " + m.PumpStation + "</font></b>");
                htw.WriteLine("<br>");
                htw.WriteLine("<b><font size='3'>Sensor - " + m.SensorName + "</font></b>");
                htw.WriteLine("<br>");
                htw.WriteLine("<b><font size='3'>Report Type - " + m.ReportType.ToString() + "</font></b>");
                htw.WriteLine("<br>");
                if (m.ReportType == ReportType.Hourly)
                {
                    htw.WriteLine("<b><font size='3'>Report Period - " + m.FromDateTime.ToShortDateString() + " Hour - " + m.Hour + "</font></b>");
                }
                else if(m.ReportType == ReportType.Realtime)
                {
                    htw.WriteLine("<b><font size='3'>Report Period - " + m.FromDateTime + "</font></b>");
                }
                else
                {
                    htw.WriteLine("<b><font size='3'>Report Period - " + m.FromDateTime.ToShortDateString() + " ~ " + m.EndDateTime.ToShortDateString() + "</font></b>");
                }
                htw.WriteLine("<br>");
                htw.WriteLine("<b><font size='3'>  </font></b>");
                htw.WriteLine("<br>");

                grid.DataBind();

                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment; filename="+ m.SensorName + "_" + m.ReportType+"_" + DateTime.Now.ToShortDateString() + ".xls");
                Response.ContentType = "application/excel";

                grid.RenderControl(htw);

                Response.Write(sw.ToString());

                Response.End();

                return Json(new { IsSuccess = true }, JsonRequestBehavior.AllowGet);

            }
            catch
            {

            }

            return Json(new { IsSuccess = false }, JsonRequestBehavior.AllowGet);
        }
        private List<ExcelDrillDownReport> GetDatewiseReportData (DrillDown m)
        {
            List<ExcelDrillDownReport> report = new List<ExcelDrillDownReport>();

            foreach (var series in m.Series)
            {
                for (int index = 0; index < series.data.Count; index++)
                {
                    var data = series.data[index];
                    ExcelDrillDownReport reportData = new ExcelDrillDownReport();
                    reportData.Sensor = series.name;
                    //reportData.Unit = m.SelectedSensor.un;//"m^3";

                    if (m.Series !=null && m.Series.Count>0 && m.Series[0].name.Contains("Water Meter"))
                    {
                        reportData.Unit = "m^3";
                    }

                    else if (m.Series != null && m.Series.Count > 0 && m.Series[0].name.Contains("Energy"))
                    {
                        reportData.Unit = "kw";
                    }
                    else
                    {
                        reportData.Unit = "";

                    }
                    reportData.Value = (data).ToString();

                    if (m.Series != null && m.Series.Count > 0 && m.Series[0].name.Contains("Water Meter"))
                    {
                        reportData.RateUnit = "Litre/min";
                    }

                    else if (m.Series != null && m.Series.Count > 0 && m.Series[0].name.Contains("Energy"))
                    {
                        reportData.RateUnit = "kw-hr";
                    }
                    else
                    {
                        reportData.RateUnit = "";

                    }
                    if (m.ReportType == ReportType.Daily)
                    {

                        reportData.Rate = (Math.Round(data / 60)).ToString();
                    }
                    else
                    {
                        reportData.Rate = (Math.Round(data / (60 * 24))).ToString();
                    }
                    //reportData.RateUnit = "Litre/min";
                    reportData.Date = m.XaxisCategory[index];

                    report.Add(reportData);
                }
            }
            return report;
        }
        private List<DrillDownReport> GetReportData (DrillDown m)
        {
            List<DrillDownReport> report = new List<DrillDownReport>();

            foreach (var series in m.Series)
            {
                for (int index = 0; index < series.data.Count; index++)
                {
                    var data = series.data[index];
                    DrillDownReport reportData = new DrillDownReport();
                    reportData.Sensor = series.name;
                    reportData.Unit = m.Unit;
                    reportData.Value = data.ToString();
                    reportData.Date = m.XaxisCategory[index];

                    report.Add(reportData);
                }
            }
            return report;
        }
        public JsonResult GetSensorDropdownData(int pumpstationId)
        {
            List<Sensor> sensorList = _reportRepository.GetSensorData(pumpstationId);
            var sendingSensorList = new List<dynamic>();
            var optiongroup=new List<string>();
            foreach (var sensor in sensorList)
            {
                dynamic sendingSensor = new ExpandoObject();
                sendingSensor.SensorID = sensor.SensorId;
                sendingSensor.Name = sensor.Name;
                sendingSensor.Model = sensor.Model;
                sendingSensor.Version = sensor.Version;
                sendingSensor.DisplayName = GetName(sensor);
                sendingSensorList.Add(sendingSensor);
                optiongroup.Add(sendingSensor.DisplayName);

            }

            return Json(new { Data = JsonConvert.SerializeObject(sendingSensorList),OptionGroup= optiongroup, IsSuccess = true }, JsonRequestBehavior.AllowGet);
        }

    private string GetName(Sensor sensor)
    {
      if (sensor is PressureSensor)
        return "Water Pressure";
      if (sensor is FlowSensor)
        return "Water Meter";
      if (sensor is EnergySensor)
        return "Energy Transmitter";
      if (sensor is LevelSensor)
        return "Level Transmitter";
      if (sensor is ACPresenceDetector)
        return "AC Presence Detector";
      if (sensor is BatteryVoltageDetector)
        return "Battery Voltage Detector";
      if (sensor is ChlorinePresenceDetector)
        return "Chlorine Presence Detector";

      return string.Empty;
    }
  }
}