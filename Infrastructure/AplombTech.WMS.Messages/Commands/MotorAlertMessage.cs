using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Messages.Commands
{
    public class MotorAlertMessage : AlertMessage
    {
        public int MotorId { get; set; }
        public string MotorName { get; set; }
        public virtual string MotorStatus { get; set; }
    }
}
