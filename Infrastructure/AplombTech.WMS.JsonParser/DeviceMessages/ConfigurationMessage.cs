using AplombTech.WMS.Domain.Devices;
using AplombTech.WMS.Domain.Motors;
using AplombTech.WMS.Domain.Sensors;
using AplombTech.WMS.JsonParser.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.JsonParser.DeviceMessages
{
    public class ConfigurationMessageEntity : DeviceMessage
    {
        public ConfigurationMessageEntity()
        {
            PumpStation=new PumpStationEntity();
            Cameras = new List<CameraEntity>();
            Sensors = new List<SensorEntity>();
            Motors = new List<MotorEntity>();
        }
        public PumpStationEntity PumpStation { get; set; }
        public IList<CameraEntity> Cameras { get; set; }
        public IList<MotorEntity> Motors { get; set; }
        public IList<SensorEntity> Sensors { get; set; }
    }
}
