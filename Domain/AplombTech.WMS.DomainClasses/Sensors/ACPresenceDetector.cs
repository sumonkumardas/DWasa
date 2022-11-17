using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Domain.Sensors
{
    [Table("ACPresenceDetectors")]
    public class ACPresenceDetector : Sensor
    {
        public string Title()
        {
            var t = Container.NewTitleBuilder();

            string title = "AC Presence Detector";

            t.Append(title);

            return t.ToString();
        }
    }
}
