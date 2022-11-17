using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace AplombTech.WMS.Data.Processor.WindowsService
{
	public partial class WmsDataProcessorService : ServiceBase
	{
		public WmsDataProcessorService ()
		{
			InitializeComponent();
		}

		protected override void OnStart (string[] args)
		{
			ServiceBus.Init("WMSSensorDataProcessor");
		}

		protected override void OnStop ()
		{
			
		}
	}
}
