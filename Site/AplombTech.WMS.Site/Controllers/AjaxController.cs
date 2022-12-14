// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System.Web.Mvc;
using System.Web.UI;
using NakedObjects.Facade;
using NakedObjects.Web.Mvc.Controllers;

namespace AplombTech.WMS.Site.Controllers {
    [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
    [Authorize]
    public class AjaxController : AjaxControllerImpl {

        public AjaxController(IFrameworkFacade facade,  IIdHelper idHelper) : base(facade, idHelper) {}

        // Uncomment this constructor if you wish to have an IDomainObjectContainer and/or domain services injected.
        // You will also need to ensure you have NakedObjects.Core package installed & add using NakedObjects;
        //public AjaxController(IFrameworkFacade facade, IIdHelper idHelper, INakedObjectsFramework nakedObjectsFramework)
        //    : base(facade, idHelper) {
        //    nakedObjectsFramework.DomainObjectInjector.InjectInto(this);
        //}

        [HttpGet]
        public override JsonResult ValidateProperty(string id, string value, string propertyName) {
            // value here is probably null - field value is extracted by id later
            return base.ValidateProperty(id, value, propertyName);
        }

        [HttpGet]
        public override JsonResult ValidateParameter(string id, string value, string actionName, string parameterName) {
            // see ValidateProperty comment
            return base.ValidateParameter(id, value, actionName, parameterName);
        }

        [HttpGet]
        public override JsonResult GetActionChoices(string id, string actionName) {
            return base.GetActionChoices(id, actionName);
        }

        [HttpGet]
        public override JsonResult GetPropertyChoices(string id) {
            return base.GetPropertyChoices(id);
        }
    }
}