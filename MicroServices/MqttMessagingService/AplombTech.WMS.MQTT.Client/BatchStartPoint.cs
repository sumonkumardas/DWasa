// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using AplombTech.WMS.AreaRepositories;
using AplombTech.WMS.DataProcessRepository;
using AplombTech.WMS.Domain.Repositories;
using AplombTech.WMS.Domain.Sensors;
using NakedObjects;
using NakedObjects.Architecture.Component;
using NakedObjects.Async;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplombTech.WMS.MQTT.Client {
    public class BatchStartPoint : IBatchStartPoint {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public IAsyncService AsyncService { private get; set; }
        public AreaRepository AreaRepository { set; protected get; }
        public ProcessRepository ProcessRepository { set; protected get; }
        public MqttClientService MqttClientService { set; protected get; }

        #region IBatchStartPoint Members
        public void Execute() {
            //AsyncService.RunAsync
            //    ((domainObjectContainer) => MqttClientFacade.MQTTClientInstance(false));

            //AsyncService.RunAsync((domainObjectContainer) =>
            //             MqttClientFacade.MQTTClientInstance(false));

            //try
            //{
            
            //ProcessRepository.CreateDataLog("sensordata", "mesage", DateTime.Now, 3);
            //RunAllProcesses();
                //log.Info("MQTT listener is going to start");
            //MqttClientService.MQTTClientInstance(false);
            //AsyncService.RunAsync((domainObjectContainer) =>
            //             ProcessRepository.CreateSensorDataLog("topic", "mesage", DateTime.Now, 3));
            //log.Info("MQTT listener has been started");
            //while (true)
            //{

            //}
            //}
            //catch (Exception ex) { Console.WriteLine(ex.ToString()); }            
        }
        #endregion
    }
}