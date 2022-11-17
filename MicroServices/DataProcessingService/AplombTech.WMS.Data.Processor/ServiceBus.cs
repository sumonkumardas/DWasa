using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Data.Processor
{
	public static class ServiceBus
	{
		public static IBus Bus { get; private set; }
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		private static readonly object PadLock = new object();

		public static void Init (string endPointName)
		{
			if (Bus != null)
				return;

			lock (PadLock)
			{
				if (Bus != null)
					return;

				var cfg = new BusConfiguration();

				cfg.UseTransport<MsmqTransport>();
				cfg.UsePersistence<InMemoryPersistence>();
				cfg.EndpointName(endPointName);
				cfg.PurgeOnStartup(false);
				cfg.EnableInstallers();

				Bus = NServiceBus.Bus.Create(cfg).Start();

				log.Info("NServiceBus Bus has been Initialized and statrted.");
			}
		}
	}
}
