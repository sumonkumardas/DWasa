// Copyright � Naked Objects Group Ltd ( http://www.nakedobjects.net). 
// All Rights Reserved. This code released under the terms of the 
// Microsoft Public License (MS-PL) ( http://opensource.org/licenses/ms-pl.html) 

using System.Web.Mvc;
using NakedObjects.Facade;
using NakedObjects.Web.Mvc.Controllers;
using NakedObjects.Web.Mvc.Models;

namespace AplombTech.WMS.Site.Controllers {

    [Authorize] 
    public class GenericController : GenericControllerImpl {

        public GenericController(IFrameworkFacade facade,  IIdHelper idHelper) : base(facade, idHelper)  {}

        // Uncomment this constructor if you wish to have an IDomainObjectContainer and/or domain services injected.
        // You will also need to ensure you have NakedObjects.Core package installed & add using NakedObjects;
        //public GenericController(IFrameworkFacade facade, IIdHelper idHelper, INakedObjectsFramework nakedObjectsFramework)
        //    : base(facade, idHelper) {
        //    nakedObjectsFramework.DomainObjectInjector.InjectInto(this);
        //}

        #region actions

        [HttpGet]
        public override ActionResult Details(ObjectAndControlData controlData) {
            return base.Details(controlData);
        }

        [HttpGet]
        public override ActionResult EditObject(ObjectAndControlData controlData) {
            return base.EditObject(controlData);
        }

        [HttpGet]
        public override ActionResult Action(ObjectAndControlData controlData) {
            return base.Action(controlData);
        }

        [HttpPost]
        public override ActionResult Details(ObjectAndControlData controlData, FormCollection form) {
            return base.Details(controlData, form);
        }

        [HttpPost]
        public override  ActionResult EditObject(ObjectAndControlData controlData, FormCollection form) {
            return base.EditObject(controlData, form);
        }

        [HttpPost]
        public override ActionResult Edit(ObjectAndControlData controlData, FormCollection form) {
            return base.Edit(controlData, form);
        }

        [HttpPost]
        public override ActionResult Action(ObjectAndControlData controlData, FormCollection form) {
            return base.Action(controlData, form);
        }

        public override FileContentResult GetFile(string Id, string PropertyId) {
            return base.GetFile(Id, PropertyId);
        }

        #endregion

    }
}