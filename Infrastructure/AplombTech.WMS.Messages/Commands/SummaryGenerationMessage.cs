using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Messages.Commands
{
    public class SummaryGenerationMessage : ICommand
    {
        public string Uid { get; set; }
        public DateTime DataLoggedAt { get; set; }
        public DateTime MessageDateTime { get; set; }
    }
}
