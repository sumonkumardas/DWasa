using AplombTech.WMS.JsonParser.DeviceMessages.Exceptions;
using AplombTech.WMS.JsonParser.Topics.Classification;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.MQTT.Client.PumpStationMessages.Processing
{
    public class MessageProcessorExceptionLoggingDecorator : IMessageProcessor
    {
        private readonly IMessageProcessor _messageProcessor;

        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public MessageProcessorExceptionLoggingDecorator(IMessageProcessor messageProcessor)
        {
            _messageProcessor = messageProcessor;
        }

        public void ProcessMessage(string topic, string message)
        {
            try
            {
                _messageProcessor.ProcessMessage(topic, message);
            }
            catch (InvalidTopicException)
            {
                Log.Error($"Topic '{topic}' is invalid");
                throw;
            }
            catch (ArgumentOutOfRangeException)
            {
                Log.Error($"Message '{message}' could not parsed correctly");
                throw;
            }
            catch (SensorMessageException ex)
            {
                Log.Error($"Invalid message while inserting in db. Throws {ex.GetType()} at {ex.StackTrace}");
                throw;
            }
            catch (Exception ex)
            {
                Log.Error($"Unknown Exception is occured {ex.GetType()} at {ex.StackTrace}");
                throw;
            }
        }
    }
}
