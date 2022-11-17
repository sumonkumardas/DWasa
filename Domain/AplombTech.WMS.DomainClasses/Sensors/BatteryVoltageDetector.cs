using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Domain.Sensors
{
    [Table("BatteryVoltageDetectors")]
    public class BatteryVoltageDetector:Sensor
    {
        public string Title()
        {
            var t = Container.NewTitleBuilder();

            string title = "Battery Voltage Detector";

            t.Append(title);

            return t.ToString();
        }
    }
}
