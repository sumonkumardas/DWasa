using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using AplombTech.WMS.QueryModel.Reports;
using AplombTech.WMS.QueryModel.Repositories;
using NakedObjects;
using NakedObjects.Facade;
using NakedObjects.Web.Mvc.Controllers;

namespace AplombTech.WMS.Site.Controllers
{
    [Authorize]
    public class UnderThresoldController : SystemControllerImpl
    {
        #region Injected Services
        public ReportRepository _reportRepository { set; protected get; }
        #endregion

        public UnderThresoldController(IFrameworkFacade facade, IIdHelper idHelper) : base(facade, idHelper) { }

        // Uncomment this constructor if you wish to have an IDomainObjectContainer and/or domain services injected.
        // You will also need to ensure you have NakedObjects.Core package installed & add using NakedObjects;
        public UnderThresoldController(IFrameworkFacade facade, IIdHelper idHelper, INakedObjectsFramework nakedObjectsFramework)
            : base(facade, idHelper)
        {
            nakedObjectsFramework.DomainObjectInjector.InjectInto(this);
        }
        // GET: UnderThresold
        public ActionResult UnderThresoldFromMap()
        {
            UnderThresold model = _reportRepository.UnderThresold();
            return View("~/Views/UnderThresold/ObjectView.cshtml", model);
        }
        [HttpPost]
        public ActionResult GetUnderThresoldReportModel(UnderThresold model)
        {
            model = _reportRepository.GetUnderThresoldtData(model);

            return PartialView("~/Views/UnderThresold/UnderThresold.cshtml", model);
        }

        public void ExportToExcel(UnderThresold m)
        {
            m = _reportRepository.GetUnderThresoldtData(m);
            //Summary model = _reportRepository.Summary();

            try
            {
                var grid = new System.Web.UI.WebControls.GridView();
                
                grid.DataSource = from d in m.UnderThresoldDatas
                                  select new
                                  {
                                      Name = m.TransmeType,
                                      Value = d.Value,
                                      Unit = m.Unit,
                                      Received = d.LoggedAt

                                  };

                grid.DataBind();

                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment; filename=" + m.ReportType + "_" + DateTime.Now.ToShortDateString() + ".xls");
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