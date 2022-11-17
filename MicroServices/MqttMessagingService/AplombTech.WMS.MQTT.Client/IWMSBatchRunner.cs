using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.MQTT.Client
{
    public interface IWMSBatchRunner
    {
        void Run(IWMSBatchStartPoint batchStartPoint);
    }
}
