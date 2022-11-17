using AplombTech.WMS.Messages.Commands;
using AplombTech.WMS.Persistence.Repositories;
using AplombTech.WMS.Persistence.UnitOfWorks;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
    using System.Threading.Tasks;

namespace AplombTech.WMS.Data.Processor
{
	public class SummarGenerator : IHandleMessages<SummaryGenerationMessage>
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public void Handle (SummaryGenerationMessage message)
		{
			log.Info("summary generation message arrived for uid=" + message.Uid + " dataloggedat=" + message.DataLoggedAt + " message datetime=" + message.MessageDateTime);
			using (WMSUnitOfWork uow = new WMSUnitOfWork())
			{
				try
				{
					ProcessRepository repo = new ProcessRepository(uow.CurrentObjectContext);
					repo.GenerateSummary(message);
					uow.CurrentObjectContext.SaveChanges();
				}
				catch (Exception ex)
				{
					log.Info("Error Occured in Handle method. Error: " + ex.Message);
				}
			}
		}
	}
}
