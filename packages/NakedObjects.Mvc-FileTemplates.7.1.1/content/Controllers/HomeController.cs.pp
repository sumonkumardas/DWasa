﻿// Copyright © Naked Objects Group Ltd ( http://www.nakedobjects.net). 
// All Rights Reserved. This code released under the terms of the 
// Microsoft Public License (MS-PL) ( http://opensource.org/licenses/ms-pl.html) 

using System.Web.Mvc;
using NakedObjects.Facade;
using NakedObjects.Web.Mvc.Controllers;
using NakedObjects.Web.Mvc.Models;

namespace $rootnamespace$.Controllers {

    //[Authorize]
    public class HomeController : SystemControllerImpl {

        public HomeController(IFrameworkFacade facade, IIdHelper idHelper) : base(facade, idHelper) {}

        // Uncomment this constructor if you wish to have an IDomainObjectContainer and/or domain services injected.
        // You will also need to ensure you have NakedObjects.Core package installed & add using NakedObjects;
        //public HomeController(IFrameworkFacade facade, IIdHelper idHelper, INakedObjectsFramework nakedObjectsFramework)
        //    : base(facade, idHelper) {
        //    nakedObjectsFramework.DomainObjectInjector.InjectInto(this);
        //}

        public ActionResult Index() {
            return View();
        }

        public ActionResult About() {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact() {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        [HttpPost]
        public override ActionResult ClearHistory(bool clearAll) {
            return base.ClearHistory(clearAll);
        }

        [HttpPost]
        public override ActionResult ClearHistoryItem(string id, string nextId, ObjectAndControlData controlData) {
            return base.ClearHistoryItem(id, nextId, controlData);
        }

        [HttpPost]
        public override ActionResult Cancel(string nextId, ObjectAndControlData controlData) {
            return base.Cancel(nextId, controlData);
        }

        [HttpPost]
        public override ActionResult ClearHistoryOthers(string id, ObjectAndControlData controlData) {
            return base.ClearHistoryOthers(id, controlData);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            if (Request.Browser.Type.ToUpper() == "IE6" || Request.Browser.Type.ToUpper() == "IE7") {
                filterContext.Result = View("BrowserError");
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}