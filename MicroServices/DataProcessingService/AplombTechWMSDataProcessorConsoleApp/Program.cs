using AplombTech.WMS.Data.Processor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTechWMSDataProcessorConsoleApp
{
	class Program
	{
		static void Main (string[] args)
		{
			ServiceBus.Init("WMSSensorDataProcessor");

			Console.ReadLine();
		}
	}
}
