using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.SensorDataFeedback.Service.Model
{
  class M2MMqttMessageModel
  {
    public string MessgeTopic { get; set; }
    public string PublishMessageStatus { get; set; }
    public string PublishMessage { get; set; }
    public string SubscriberMessage { get; set; }
    public string SubscribehMessageStatus { get; set; }
    public string ReceivedMessage { get; set; }
  }
}
