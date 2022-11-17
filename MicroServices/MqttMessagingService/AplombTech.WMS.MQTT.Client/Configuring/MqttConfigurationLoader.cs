using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace AplombTech.WMS.MQTT.Client.Configuring
{
    public class MqttConfigurationLoader
    {
        protected string DefaultBrokerAddress = string.Empty;
        protected int DefaultBrokerPort = 1883;
        protected ushort DefaultBrokerKeepAlivePeriod = 3600;
        protected string DefaultClientId = Guid.NewGuid().ToString();

        public MqttConfiguration LoadMqttConfiguration()
        {
            return new MqttConfiguration
            {
                BrokerAddress = GetBrokerAddress(),
                BrokerPort = GetBrokerPort(),
                BrokerKeepAlivePeriod = GetBrokerKeepAlivePeriod(),
                ClientId = GetClientId(),
                IsSsl = GetIsSsl(),
                Topics = GetTopics(),
                QosLevel = GetOosLevel()
            };
        }

        protected string GetBrokerAddress()
        {
            return ReadAppSettings("BrokerAddress") ?? DefaultBrokerAddress;
        }

        protected int GetBrokerPort()
        {
            return ReadAppSettings("BrokerPort") == null
                ? DefaultBrokerPort
                : Convert.ToInt32(ReadAppSettings("BrokerPort"));
        }

        protected ushort GetBrokerKeepAlivePeriod()
        {
            return ReadAppSettings("BrokerKeepAlivePeriod") == null
                ? DefaultBrokerKeepAlivePeriod
                : Convert.ToUInt16(ReadAppSettings("BrokerKeepAlivePeriod"));
        }

        protected string GetClientId()
        {
            return ReadAppSettings("BrokerAccessClientId") ?? DefaultClientId;
        }

        protected string ReadAppSettings(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        protected bool GetIsSsl()
        {
#if DEBUG
            return false;
#else
            return true;
#endif
        }

        protected string[] GetTopics()
        {
            return new[] { "E/1/DAT", "E/1/TKN/FB" };
        }

        protected byte GetOosLevel()
        {
            return MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE;
        }
    }
}
