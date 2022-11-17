// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using NakedObjects.Core.Configuration;
using NakedObjects.Persistor.Entity.Configuration;
using NakedObjects.Architecture.Menu;
using NakedObjects.Menu;
using NakedObjects.Core.Async;
using AplombTech.WMS.Domain.Repositories;
//using AplombTech.WMS.Domain.Facade;
using AplombTech.WMS.Domain.Motors;
using AplombTech.WMS.Domain.Sensors;
using AplombTech.WMS.AreaBoundedContext;
using AplombTech.WMS.AreaRepositories;
using AplombTech.WMS.DataProcessBoundedContext;
using AplombTech.WMS.DataProcessRepository;
using AplombTech.WMS.SensorDataLogBoundedContext.Facade;
using AplombTech.WMS.CompanyDatabase;

namespace AplombTech.WMS.MQTT.Client
{

	// Use this class to configure the application running under Naked Objects
	public class NakedObjectsRunSettings
	{

		private static string[] ModelNamespaces
		{
			get { return new string[] {"AplombTech.WMS.Domain"}; //Add top-level namespace(s) that cover the domain model
			}
		}

		private static Type[] Services
		{
			get
			{
				return new Type[]
				{
					typeof (AsyncService),
					//Add your domain services here
					typeof (AreaRepository),
					typeof (ProcessRepository)
					//typeof(MqttClientService)
				};
			}
		}

		// Specify any types that need to be reflected-over by the framework and that
		// will not be discovered via the services
		private static Type[] Types
		{
			get
			{
				return new Type[]
				{
					typeof (Sensor),
					typeof (EnergySensor),
					typeof (FlowSensor),
					typeof (LevelSensor),
					typeof (PressureSensor),
					typeof (ChlorinePresenceDetector),
					typeof (ACPresenceDetector),
					typeof (BatteryVoltageDetector),
					typeof (Motor),
					typeof (PumpMotor),
					typeof (ChlorineMotor),
					typeof (Domain.SummaryData.UnderThresoldData)
					//typeof(SensorData),
					//typeof(MotorData),
					//typeof(DataLog)
				};
			}
		}

		public static ReflectorConfiguration ReflectorConfig()
		{
			return new ReflectorConfiguration(Types, Services, ModelNamespaces, MainMenus);
		}

		public static EntityObjectStoreConfiguration EntityObjectStoreConfig()
		{
			var config = new EntityObjectStoreConfiguration();
			//config.UsingCodeFirstContext(() => new CompanyDatabaseContext());
			config.UsingCodeFirstContext(() => new SensorDataLogContext());
			config.UsingCodeFirstContext(() => new AreaContext());
			//config.UsingCodeFirstContext(() => new ProcessContext());
			return config;
		}

		/// <summary>
		/// Return an array of IMenus (obtained via the factory, then configured) to
		/// specify the Main Menus for the application. If none are returned then
		/// the Main Menus will be derived automatically from the MenuServices.
		/// </summary>
		public static IMenu[] MainMenus(IMenuFactory factory)
		{
			var areaMenu = factory.NewMenu<AreaRepository>();
			AreaRepository.Menu(areaMenu);

			return new IMenu[]
			{
				//factory.NewMenu<CentralKhelaGharRepository>(true),
				//asarMenu,
				areaMenu
			};
		}
	}
}