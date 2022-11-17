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
    public class MotorOnOffController : SystemControllerImpl
    {
        #region Injected Services
        public ReportRepository _reportRepository { set; protected get; }
        #endregion

        public MotorOnOffController(IFrameworkFacade facade, IIdHelper idHelper) : base(facade, idHelper) { }

        // Uncomment this constructor if you wish to have an IDomainObjectContainer and/or domain services injected.
        // You will also need to ensure you have NakedObjects.Core package installed & add using NakedObjects;
        public MotorOnOffController(IFrameworkFacade facade, IIdHelper idHelper, INakedObjectsFramework nakedObjectsFramework)
            : base(facade, idHelper)
        {
            nakedObjectsFramework.DomainObjectInjector.InjectInto(this);
        }

        [HttpPost]
        public ActionResult GetMotorOnOffReportModel(MotorOnOff model)
        {
          model = _reportRepository.GenerateMotorOnOffData(model);

          return PartialView("~/Views/MotorOnOff/MotorOnOff.cshtml", model);
        }
  }
}