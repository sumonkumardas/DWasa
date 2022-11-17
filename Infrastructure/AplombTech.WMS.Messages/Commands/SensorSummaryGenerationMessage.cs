using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Messages.Commands
{
    public class SensorSummaryGenerationMessage : SummaryGenerationMessage
    {
        public decimal Value { get; set; }
        public decimal SecondaryValue { get; set; }
    }
}
