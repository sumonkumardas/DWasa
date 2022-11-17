using AplombTech.WMS.Domain.Sensors;
using AplombTech.WMS.SensorDataLogBoundedContext.Facade;
using AplombTech.WMS.Utility.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.MQTT.Client.PumpStationMessages.Processing
{
    public class MessageProcessorDbLogDecorator : IMessageProcessor
    {
        private readonly IMessageProcessor _messageProcessor;
        private readonly IDbContextRunner _dbContextRunner;

        public MessageProcessorDbLogDecorator(IMessageProcessor messageProcessor, IDbContextRunner dbContextRunner)
        {
            _messageProcessor = messageProcessor;
            _dbContextRunner = dbContextRunner;
        }

        public void ProcessMessage(string topic, string message)
        {            
             DataLog log =_dbContextRunner.RunAndSaveInDbContext<SensorDataLogContext, DataLog>(
                context=> InsertMeterDataLog(context, topic, message));
            try
            {
                _messageProcessor.ProcessMessage(topic, message);
            }
            catch (Exception ex)
            {
                var exceptionMessage = GetFormattedExceptionMessage(ex);
                _dbContextRunner.RunAndSaveInDbContext<SensorDataLogContext>(
                    context => UpdateIsProcessed(context, log.SensorDataLogID, DataLog.ProcessingStatusEnum.Failed, exceptionMessage));
                throw;
            }
            _dbContextRunner.RunAndSaveInDbContext<SensorDataLogContext>(
                context => UpdateIsProcessed(context, log.SensorDataLogID, processed: DataLog.ProcessingStatusEnum.Done, exceptionMessage: null));
        }

        private DataLog InsertMeterDataLog(SensorDataLogContext dbContext, string topic, string message)
        {
            var m = new DataLog
            {
                Topic = topic,
                Message = message,
                MessageReceivedAt = DateTime.Now,
                //LoggedAtSensor = message.
                ProcessingStatus = DataLog.ProcessingStatusEnum.None
            };
            dbContext.DataLogs.Add(m);
            return m;
        }

        private void UpdateIsProcessed(SensorDataLogContext dbContext, long logId, DataLog.ProcessingStatusEnum processed,
            string exceptionMessage)
        {
            var mqttMessageLogs = dbContext.DataLogs.Where(log => log.SensorDataLogID == logId);
            var mqttMessageLog = mqttMessageLogs.FirstOrDefault();

            if (mqttMessageLog != null)
            {
                mqttMessageLog.ProcessingStatus = processed;
                mqttMessageLog.ExceptionMessage = exceptionMessage;
            }
        }

        private string GetFormattedExceptionMessage(Exception ex)
        {
            return $"Type is {ex.GetType()}, Message is {ex.Message}, Stacktrace is {ex.StackTrace}";
        }
    }
}
