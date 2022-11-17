using AplombTech.WMS.QueryModel.Repositories;
using NakedObjects;
using NakedObjects.Facade;
using NakedObjects.Web.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using AplombTech.WMS.Domain.Areas;
using AplombTech.WMS.QueryModel.Reports;
using AplombTech.WMS.Domain.Sensors;
using Newtonsoft.Json;
using AplombTech.WMS.Domain.Sensors;

namespace AplombTech.WMS.Site.Controllers
{
    [Authorize]
    public class SummaryController : SystemControllerImpl
    {
        #region Injected Services
        public ReportRepository _reportRepository { set; protected get; }
        #endregion

        public SummaryController(IFrameworkFacade facade, IIdHelper idHelper) : base(facade, idHelper) { }

        // Uncomment this constructor if you wish to have an IDomainObjectContainer and/or domain services injected.
        // You will also need to ensure you have NakedObjects.Core package installed & add using NakedObjects;
        public SummaryController(IFrameworkFacade facade, IIdHelper idHelper, INakedObjectsFramework nakedObjectsFramework)
            : base(facade, idHelper)
        {
            nakedObjectsFramework.DomainObjectInjector.InjectInto(this);
        }

        // GET: ScadaMap
        public ActionResult Index()
        {
            //ZoneMap zones = _reportRepository.GoogleMap();
            //int totalZone = zones.Zones.Count();
            return View();
        }

        // GET: ScadaMap/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ScadaMap/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ScadaMap/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ScadaMap/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ScadaMap/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ScadaMap/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ScadaMap/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public void ExportToExcel(Summary m)
        {
            Summary model = _reportRepository.Summary();
            List<Sensor> sensors = new List<Sensor>();
            foreach (var zone in model.Zones)
            {
                foreach (var dma in zone.DMAs)
                {
                    foreach (var pumpStation in dma.PumpStations)
                    {
                        foreach (var sensor in pumpStation.Sensors)
                        {
                            sensors.Add(sensor);
                        }
                    }
                }
            }
            try
            {
                var grid = new System.Web.UI.WebControls.GridView();

                grid.DataSource = from d in sensors
                                  select new
                                  {
                                      Name = d.GetType().ToString().Split('.')[4].Split('_')[0],
                                      CurrentValue = d.CurrentValue,
                                      Dma = d.PumpStation.Parent.Name,
                                      PumpStation = d.PumpStation.Name,
                                      ReceivedTime=d.LastDataReceived

                                  };

                grid.DataBind();

                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment; filename=Overview_"+DateTime.Now.ToShortDateString()+".xls");
                Response.ContentType = "application/excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                grid.RenderControl(htw);

                Response.Write(sw.ToString());

                Response.End();
            }
            catch
            {
                
            }
        }

    }
}
