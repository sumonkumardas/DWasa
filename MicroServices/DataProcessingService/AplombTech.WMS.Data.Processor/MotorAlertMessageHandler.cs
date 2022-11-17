using AplombTech.WMS.Domain.Alerts;
using AplombTech.WMS.Messages.Commands;
using AplombTech.WMS.Persistence.Repositories;
using AplombTech.WMS.Persistence.UnitOfWorks;
using AplombTech.WMS.Utility;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Data.Processor
{
	public class MotorAlertMessageHandler : IHandleMessages<MotorAlertMessage>
	{
		public void Handle (MotorAlertMessage message)
		{
			string alertMessage = String.Empty;

			IList<AlertRecipient> recipients = new List<AlertRecipient>();
			using (WMSUnitOfWork uow = new WMSUnitOfWork())
			{
				AlertConfigurationRepository repo = new AlertConfigurationRepository(uow.CurrentObjectContext);
				alertMessage = repo.GetMessageByAlertMessageTypeId(message.AlertMessageType);
				recipients = repo.GetReceipientsByAlertTypeId(message.AlertMessageType);
			}

			HandleMotorMessage(message, alertMessage, recipients);
		}

		private void HandleMotorMessage (MotorAlertMessage motorMessage, string alertMessage, IList<AlertRecipient> recipients)
		{
			using (WMSUnitOfWork uow = new WMSUnitOfWork())
			{
				string[] messageList = alertMessage.Split('|');

				string message = motorMessage.MotorName + " " + messageList[0] + " " + motorMessage.PumpStationName + " is " + motorMessage.MotorStatus;
				AlertConfigurationRepository repo = new AlertConfigurationRepository(uow.CurrentObjectContext);

				foreach (AlertRecipient recipient in recipients)
				{
					AlertLog log = new AlertLog
					{
						AlertGereratedObjectId = motorMessage.MotorId,
						MessageDateTime = DateTime.Now,
						AlertMessageType = (int)AlertType.AlertTypeEnum.OnOff
					};

					if (recipient.Email.Trim().Length > 0)
					{
						log.ReciverEmail = recipient.Email;
						EmailSender.SendEmail(recipient.Email, "Pump Motor Status", message);
					}
					if (recipient.MobileNo.Trim().Length > 0)
					{
						log.ReciverMobileNo = recipient.MobileNo;
						SmsSender.SendSMS(recipient.MobileNo, message);
					}
					repo.SaveAlertLog(log);
				}

				uow.CurrentObjectContext.SaveChanges();
			}
		}
	}
  
}
