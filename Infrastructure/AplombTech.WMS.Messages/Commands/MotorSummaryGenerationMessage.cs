using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Messages.Commands
{
    public class MotorSummaryGenerationMessage : SummaryGenerationMessage
    {
        public string MotorStatus { get; set; }
    }
}
