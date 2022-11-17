using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.MQTT.WindowsService
{
  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    static void Main(string []args)
    {
			ServiceBase[] ServicesToRun;
			ServicesToRun = new ServiceBase[]
			{
								new WmsMqttService(), 
			};
			ServiceBase.Run(ServicesToRun);
		}
  }
}
