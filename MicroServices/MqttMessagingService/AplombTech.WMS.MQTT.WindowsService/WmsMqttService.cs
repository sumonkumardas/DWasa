using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace AplombTech.WMS.MQTT.WindowsService
{
  public partial class WmsMqttService : ServiceBase
  {
		MqttClientService _mqttService = new MqttClientService();
		public WmsMqttService()
    {
      InitializeComponent();
    }

    protected override void OnStart(string[] args)
    {
			ServiceBus.Init("WMSSensorDataSender");

			_mqttService.MqttClientInstance(false);
		}

    protected override void OnStop()
    {
	    _mqttService.DisconnectBroker();

    }
  }
}
