using AplombTech.WMS.Domain.Shared;
using NakedObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Domain.Sensors
{
    [Table("PressureSensors")]
    public class PressureSensor : Sensor
    {
        public string Title()
        {
            var t = Container.NewTitleBuilder();

            string title = "Pressure Sensor";

            t.Append(title);

            return t.ToString();
        }

        //[MemberOrder(80)]
        //public virtual Unit Unit { get; set; }
    }
}
