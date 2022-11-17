using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.WMS.Domain.Motors;
using AplombTech.WMS.Domain.Sensors;

namespace AplombTech.WMS.QueryModel.Shared
{
    public class ScadaViewModel
    {
        public ScadaViewModel()
        {
            SensorList = new List<Sensor>();
            MotorDataList = new List<MotorData>();
        }
        public List<Sensor> SensorList { get; set; }
        public List<MotorData> MotorDataList { get; set; }
    }
}
