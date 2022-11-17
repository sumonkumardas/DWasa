using AplombTech.MQTTLib;
using AplombTech.WMS.Messages;
using AplombTech.WMS.Site.MQTT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace AplombTech.WMS.Site
{
  
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
      //MqttClientFacade.MQTTClientInstance(false).MakeConnection();
      //#if DEBUG
      //             MQTTService.MQTTClientInstance(true).MakeConnection();
      //#else
      //            MQTTService.MQTTClientInstance(true).MakeConnection();
      //#endif
      //ServiceBus.Initialize("TokenMessageSender");
      log4net.Config.XmlConfigurator.Configure(new FileInfo(Server.MapPath("~/Web.config")));
      MQTTService.MQTTClientInstance(false).MakeConnection();
      AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
