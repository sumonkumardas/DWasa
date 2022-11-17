using AplombTech.WMS.Persistence.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.WMS.Domain.Alerts;

namespace AplombTech.WMS.Persistence.Repositories
{
    public class AlertConfigurationRepository
    {
        private readonly WMSDBContext _wmsdbcontext;

        public AlertConfigurationRepository(WMSDBContext wmsdbcontext)
        {
            _wmsdbcontext = wmsdbcontext;
        }

        public string GetMessageByAlertMessageTypeId(int alertTypeId)
        {
            AlertType type = (from c in _wmsdbcontext.AlertTypes where c.AlertTypeId == alertTypeId select c).FirstOrDefault();
            if (type != null)
                return type.AlertMessage;

            return String.Empty;
        }

        public AlertLog GetAlertLog(int alertObjectId,int year,int month,int day,int hour)
        {
            return (from c in _wmsdbcontext.AlertLogs where c.AlertGereratedObjectId == alertObjectId && c.MessageDateTime.Year == year && c.MessageDateTime.Month == month && c.MessageDateTime.Day == day  && c.MessageDateTime.Hour == hour select c).FirstOrDefault();
            
        }

        public void SaveAlertLog(AlertLog alertLog)
        {
            _wmsdbcontext.AlertLogs.Add(alertLog);
        }

        public IList<AlertRecipient> GetReceipientsByAlertTypeId(int alertTypeId)
        {
            AlertType type=  (from c in _wmsdbcontext.AlertTypes where c.AlertTypeId == alertTypeId select c).Single();

            return type.AlertRecipients.ToList();
        }
    }
}
