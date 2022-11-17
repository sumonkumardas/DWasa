using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Messages.Commands
{
    public class AlertMessage : ICommand
    {        
        public string PumpStationName { get; set; }
        public int AlertMessageType { get; set; }
        public DateTime MessageDateTime { get; set; }
    }
}
