using AplombTech.WMS.JsonParser.DeviceMessages.Parsing;
using AplombTech.WMS.JsonParser.Topics.Classification;
using AplombTech.WMS.MQTT.Client;
using NakedObjects;
using NakedObjects.Architecture.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NakedObjects.Core.Component
{
    public sealed class WMSBatchRunner : IWMSBatchRunner
    {
        private readonly INakedObjectsFramework framework;

        public WMSBatchRunner(INakedObjectsFramework framework)
        {
            //Assert.AssertNotNull(framework);
            this.framework = framework;
        }

        #region IBatchRunner Members

        public void Run(IWMSBatchStartPoint batchStartPoint)
        {
            framework.DomainObjectInjector.InjectInto(batchStartPoint);
            //StartTransaction();
            batchStartPoint.Execute(framework);
            //EndTransaction();
            while (true)
            {
                Thread.Sleep(5000);
            }
        }

        #endregion

        private void StartTransaction()
        {
            framework.TransactionManager.StartTransaction();
        }

        private void EndTransaction()
        {
            framework.TransactionManager.EndTransaction();
        }
    }
}