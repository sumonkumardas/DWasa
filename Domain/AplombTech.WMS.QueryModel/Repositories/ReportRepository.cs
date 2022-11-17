using AplombTech.WMS.Domain.Areas;
using AplombTech.WMS.QueryModel.Reports;
using NakedObjects.Menu;
using NakedObjects.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.WMS.Domain.Sensors;
using AplombTech.WMS.Domain.Shared;
using AplombTech.WMS.Domain.UserAccounts;
using AplombTech.WMS.Domain.Features;
using AplombTech.WMS.Domain.Motors;
using NakedObjects.Core.Util.Enumer;
using AplombTech.WMS.Domain.SummaryData;
using AplombTech.WMS.Domain.Repositories;
using AplombTech.WMS.Domain.Vfds;
using AplombTech.WMS.QueryModel.Shared;
using AplombTech.WMS.SensorDataLogBoundedContext.Repositories;
using AplombTech.WMS.SensorDataLogBoundedContext.UnitOfWorks;

namespace AplombTech.WMS.QueryModel.Repositories
{
	[DisplayName("Reports")]
	public class ReportRepository : AbstractFactoryAndRepository
	{
		#region Injected Services

		public LoggedInUserInfoDomainRepository LoggedInUserInfoRepository { set; protected get; }

		#endregion

		public static void Menu(IMenu menu)
		{
			menu.AddAction("GoogleMap");
			menu.AddAction("ScadaMap");
			menu.AddAction("DrillDown");
			menu.AddAction("Summary");
			menu.AddAction("UnderThresold");
      menu.AddAction("MotorOnOff");
    }

		[DisplayName("Google Map")]
		public ZoneGoogleMap GoogleMap()
		{
			var zones = Container.NewViewModel<ZoneGoogleMap>();
			zones.Zones = Container.Instances<Zone>().ToList();

			return zones;
		}

		public ZoneGoogleMap GetSingleAreaGoogleMap(int zoneId)
		{
			var zones = Container.NewViewModel<ZoneGoogleMap>();
			zones.Zones = Container.Instances<Zone>().Where(x => x.AreaId == zoneId).ToList();

			return zones;
		}

		public bool HideUnderGoogleMap()
		{
			IList<Feature> features = LoggedInUserInfoRepository.GetFeatureListByLoginUser();

			Feature feature =
				features.Where(w => w.FeatureCode == (int) Feature.ReportFeatureEnums.GoogleMap
				                    && w.FeatureType.FeatureTypeName == FeatureType.FeatureTypeEnums.Report.ToString())
					.FirstOrDefault();

			if (feature == null)
				return true;
			return false;
		}

		[DisplayName("Drill Down")]
		public DrillDown DrillDown()
		{
			var model = Container.NewViewModel<DrillDown>();
			model.PumpStations = Container.Instances<PumpStation>().ToList();

			return model;
		}

		public bool HideUnderDrillDown()
		{
			IList<Feature> features = LoggedInUserInfoRepository.GetFeatureListByLoginUser();

			Feature feature =
				features.Where(w => w.FeatureCode == (int) Feature.ReportFeatureEnums.DrillDown
				                    && w.FeatureType.FeatureTypeName == FeatureType.FeatureTypeEnums.Report.ToString())
					.FirstOrDefault();

			if (feature == null)
				return true;
			return false;
		}

		[DisplayName("Under Thresold")]
		public UnderThresold UnderThresold()
		{
			var model = Container.NewViewModel<UnderThresold>();
			model.PumpStations = Container.Instances<PumpStation>().ToList();

			return model;
		}

		public bool HideUnderThresold()
		{
			IList<Feature> features = LoggedInUserInfoRepository.GetFeatureListByLoginUser();

			Feature feature =
				features.Where(w => w.FeatureCode == (int) Feature.ReportFeatureEnums.UnderThreshold
				                    && w.FeatureType.FeatureTypeName == FeatureType.FeatureTypeEnums.Report.ToString())
					.FirstOrDefault();

			if (feature == null)
				return true;
			return false;
		}

		public Summary Summary()
		{
			var model = Container.NewViewModel<Summary>();
			model.Zones = Container.Instances<Zone>().ToList();

			return model;
		}

		public bool HideSummary()
		{
			IList<Feature> features = LoggedInUserInfoRepository.GetFeatureListByLoginUser();

			Feature feature =
				features.Where(w => w.FeatureCode == (int) Feature.ReportFeatureEnums.SummaryReport
				                    && w.FeatureType.FeatureTypeName == FeatureType.FeatureTypeEnums.Report.ToString())
					.FirstOrDefault();

			if (feature == null)
				return true;
			return false;
		}

		public ScadaMap ScadaMap()
		{
			var model = Container.NewViewModel<ScadaMap>();
			model.Zones = Container.Instances<Zone>().ToList();
			return model;
		}

		public bool HideScadaMap()
		{
			IList<Feature> features = LoggedInUserInfoRepository.GetFeatureListByLoginUser();

			Feature feature =
				features.Where(w => w.FeatureCode == (int) Feature.ReportFeatureEnums.ScadaMap
				                    && w.FeatureType.FeatureTypeName == FeatureType.FeatureTypeEnums.Report.ToString())
					.FirstOrDefault();

			if (feature == null)
				return true;
			return false;
		}

    [DisplayName("Motor On Off")]
    public MotorOnOff MotorOnOff()
    {
      var model = Container.NewViewModel<MotorOnOff>();
      model.PumpStations = Container.Instances<PumpStation>().ToList();

      return model;
    }

    public bool HideUnderMotorOnOff()
    {
      IList<Feature> features = LoggedInUserInfoRepository.GetFeatureListByLoginUser();

      Feature feature =
        features.Where(w => w.FeatureCode == (int)Feature.ReportFeatureEnums.MotorOnOff
                            && w.FeatureType.FeatureTypeName == FeatureType.FeatureTypeEnums.Report.ToString())
          .FirstOrDefault();

      if (feature == null)
        return true;
      return false;
    }

    public List<DMA> GetDmaList(int zoneId)
		{
			var model = Container.Instances<DMA>().Where(x => x.Parent.AreaId == zoneId).ToList();
			return model;
		}

		public List<PumpStation> GetPumpStationList(int dmaId)
		{
			var model = Container.Instances<PumpStation>().Where(x => x.Parent.AreaId == dmaId).ToList();
			return model;
		}

		public List<Sensor> GetSensorData(int pumpStationId)
		{
			PumpStation pumpStation = Container.Instances<PumpStation>().Where(x => x.AreaId == pumpStationId).FirstOrDefault();
			//var temp = pumpStation.Sensors.ToList();
			return pumpStation.Sensors.ToList();
		}

		public PumpMotor GetPumpMotorData(int pumpStationId)
		{
			PumpStation pumpStation = Container.Instances<PumpStation>().Where(x => x.AreaId == pumpStationId).FirstOrDefault();

			PumpMotor motor = pumpStation.PumpMotors;
			if (motor == null) return null;
			else return motor;

		}

		public VariableFrequencyDrive GetVFDData(int pumpStationId)
		{
			return
				Container.Instances<VariableFrequencyDrive>().Where(w => w.PumpStation.AreaId == pumpStationId).FirstOrDefault();
		}

		public ChlorineMotor GetCholorineMotorData(int pumpStationId)
		{
			using (SDLUnitOfWork uow = new SDLUnitOfWork())
			{
				PumpStation pumpStation =
					Container.Instances<PumpStation>().Where(x => x.AreaId == pumpStationId).FirstOrDefault();

				ChlorineMotor motor = pumpStation.ChlorineMotors;
				if (motor == null) return null;
				else return motor;
			}
		}

		public Motor GetMotor(int motorId)
		{
			return
				Container.Instances<Motor>().Where(x => x.MotorID == motorId).FirstOrDefault();


		}

		public Sensor GetPumpSingleSensor(int SensorId)
		{
			var model = Container.Instances<Sensor>().Where(x => x.SensorId == SensorId).SingleOrDefault();
			return model;
		}

		public Sensor GetPumpSingleSensorByUid(string uid)
		{
			var model = Container.Instances<Sensor>().Where(x => x.UUID == uid).SingleOrDefault();
			return model;
		}

		public PumpStation GetPumpStationById(int pumpStationId)
		{
			return Container.Instances<PumpStation>().Where(x => x.AreaId == pumpStationId).SingleOrDefault();
		}

		public Dictionary<string, string> GetPumpStationOverView(int pumpStationId)
		{
			var model = Container.Instances<PumpStation>().Where(x => x.AreaId == pumpStationId).FirstOrDefault();
			Dictionary<string, string> dictonary = new Dictionary<string, string>();
			GetSensorDataDictonary(model, dictonary);

			return dictonary;

		}

		private Dictionary<string, string> GetSensorDataDictonary(PumpStation model, Dictionary<string, string> dictonary)
		{
			foreach (var sensor in model.Sensors)
			{
				if (sensor is PressureSensor)
				{
					var unitName = sensor.UnitName;
					dictonary.Add("PT-" + sensor.UUID, sensor.CurrentValue + " " + unitName);
				}


				else if (sensor is LevelSensor)
				{
					var unitName = sensor.UnitName;
					dictonary.Add("LT-" + sensor.UUID, sensor.CurrentValue + " " + unitName);
				}

				else if (sensor is EnergySensor)
				{
					var unitName = sensor.UnitName;
					dictonary.Add("ET-" + sensor.UUID, sensor.CurrentValue + " " + unitName);
				}

				else if (sensor is FlowSensor)
				{
					var unitName = sensor.UnitName;
					dictonary.Add("FT-" + sensor.UUID, sensor.CurrentValue + " " + unitName);
				}

				else if (sensor is ChlorinePresenceDetector)
				{
					string cholorinationValue = null;
					cholorinationValue = sensor.CurrentValue == 0 ? "Cholorination on" : "Cholorination off";
					dictonary.Add("CPD-" + sensor.UUID, cholorinationValue);
				}
				else if (sensor is ACPresenceDetector)
				{
					string acpValue = null;
					acpValue = sensor.CurrentValue == 0 ? "ACP on" : "ACP off";
					dictonary.Add("ACP-" + sensor.UUID, acpValue);
				}
				else if (sensor is BatteryVoltageDetector)
				{
					var unitName = sensor.UnitName;
					dictonary.Add("BV-" + sensor.UUID, sensor.CurrentValue + " " + unitName);
				}
			}

			return dictonary;
		}

		#region DrillDown Report Data

		public DrillDown GetReportData(DrillDown model)
		{
            model.SelectedSensor = GetPumpSingleSensor(model.SelectedSensor.SensorId);
            model.SensorName = model.SelectedSensor.Name;
            model.Zone = model.SelectedSensor.PumpStation.Parent.Parent.Name;
            model.DMA = model.SelectedSensor.PumpStation.Parent.Name;
            model.PumpStation = model.SelectedSensor.PumpStation.Name;

            if (model.ReportType == ReportType.Daily)
			{
				model.FromDateTime = new DateTime(model.Year, (int) model.Month, model.Day);
                model.EndDateTime = model.FromDateTime;
                return GeneratetSeriesDataDaily(model);
			}
			if (model.ReportType == ReportType.Hourly)
			{
				model.FromDateTime = new DateTime(model.Year, (int) model.Month, model.Day, model.Hour - 1, 0, 0);
                model.EndDateTime = new DateTime(model.Year, (int)model.Month, model.Day, model.Hour - 1, 59, 59);
                return GeneratetSeriesDataHourly(model);
			}
			if (model.ReportType == ReportType.Weekly)
			{
				model.FromDateTime = new DateTime(model.Year, 1, 1).AddDays((model.Week - 1)*7);
                model.EndDateTime = model.FromDateTime.AddDays(7);
                return GeneratetSeriesDataWeekly(model);
			}

			if (model.ReportType == ReportType.Monthly)
			{
				model.FromDateTime = new DateTime(model.Year, (int) model.Month, 1);
                model.EndDateTime = model.FromDateTime.AddMinutes(1).AddDays(-1);
                return GeneratetSeriesDataMonthly(model);
			}

			if (model.ReportType == ReportType.Realtime)
			{
				model.FromDateTime = DateTime.Now;
                model.EndDateTime = model.FromDateTime;
                return GeneratetSeriesDataRealTime(model);
			}

			return null;
		}

		public UnderThresold GetUnderThresoldtData(UnderThresold model)
		{
			if (model.ReportType == ReportType.Daily)
			{
				model.FromDateTime = new DateTime(model.Year, (int) model.Month, model.Day, 0, 0, 0);
				model.ToDateTime = new DateTime(model.Year, (int) model.Month, model.Day, 23, 59, 59);

			}
			if (model.ReportType == ReportType.Hourly)
			{
				model.FromDateTime = new DateTime(model.Year, (int) model.Month, model.Day, model.Hour - 1, 0, 0);
				model.ToDateTime = new DateTime(model.Year, (int) model.Month, model.Day, model.Hour - 1, 59, 59);

			}
			if (model.ReportType == ReportType.Weekly)
			{
				model.FromDateTime = new DateTime(model.Year, 1, 1).AddDays((model.Week - 1)*7);
				model.ToDateTime = model.FromDateTime.AddDays(7);
			}

			if (model.ReportType == ReportType.Monthly)
			{
				model.FromDateTime = new DateTime(model.Year, (int) model.Month, 1);
				model.ToDateTime = model.FromDateTime.AddDays(DateTime.DaysInMonth(model.Year, (int) model.Month));

			}
			return GenerateUnderThresoldData(model);
		}

		private UnderThresold GenerateUnderThresoldData(UnderThresold model)
		{
			PumpStation pumpStation =
				Container.Instances<PumpStation>().Where(w => w.AreaId == model.SelectedPumpStationId).First();
			Sensor sensor = GetPumpStationSensor<Sensor>(pumpStation, ref model);
			model.UnderThresoldDatas = GetCurrentDataListWithinTime(sensor, model.FromDateTime, model.ToDateTime);
			return model;
		}

		private DrillDown GeneratetSeriesDataRealTime(DrillDown model)
		{
			SetGraphTitleAndSubTitle(ref model, "Live Data Review", null);
			PumpStation pumpStation =
				Container.Instances<PumpStation>().Where(w => w.AreaId == model.SelectedPumpStationId).First();

			return SetupLiveData(model);
		}

		private DrillDown GeneratetSeriesDataDaily(DrillDown model)
		{
			SetGraphTitleAndSubTitle(ref model, "Daily Data Review", "Data for " + model.FromDateTime.DayOfWeek);
			model.SelectedSensor = GetPumpSingleSensor(model.SelectedSensor.SensorId);
			return SetupDailyData(model);
		}

		private DrillDown SetupDailyData(DrillDown model)
		{
			ReportSeries data = new ReportSeries();
			data.name = GetName(model.SelectedSensor);
			data.data = GetDailyData(ref model);
			model.Unit = model.SelectedSensor.UnitName;
			model.Series.Add(data);
			model.SelectedSensor = new Sensor()
			{
				SensorId = model.SelectedSensor.SensorId,
				MinimumValue = model.SelectedSensor.MinimumValue
			};
			return model;
		}

    private string GetName(Sensor sensor)
    {
      if (sensor is PressureSensor)
        return "Water Pressure";
      if (sensor is FlowSensor)
        return "Water Meter";
      if (sensor is EnergySensor)
        return "Energy Transmitter";
      if (sensor is LevelSensor)
        return "Level Transmitter";
      if (sensor is ACPresenceDetector)
        return "AC Presence Detector";
      if (sensor is BatteryVoltageDetector)
        return "Battery Voltage Detector";
      if (sensor is ChlorinePresenceDetector)
        return "Chlorine Presence Detector";

      return string.Empty;
    }

    private DrillDown SetupLiveData(DrillDown model)
		{
			var sensor = GetPumpSingleSensor(model.SelectedSensor.SensorId);
			ReportSeries data = new ReportSeries();
			data.name = GetName(model.SelectedSensor);
			model.Unit = model.SelectedSensor.UnitName;
			model.SelectedSensor = new Sensor() {SensorId = model.SelectedSensor.SensorId};
			//if (sensor is EnergySensor)
			//{
			//    data.data = new List<double>() { (double)((EnergySensor)sensor) };
			//}
			//else if (sensor is FlowSensor)
			//{
			//    data.data = new List<double>() { (double)sensor.CurrentValue };
			//}
			//else
			data.data = new List<double>() {(double) sensor.CurrentValue};
			model.Series.Add(data);
			return model;
		}

		private DrillDown GeneratetSeriesDataHourly(DrillDown model)
		{
			SetGraphTitleAndSubTitle(ref model, "Hourly Data Review",
				"Data between Hour no = " + model.FromDateTime.Hour + " to " + ((model.FromDateTime.Hour) + 1));
			model.SelectedSensor = GetPumpSingleSensor(model.SelectedSensor.SensorId);
			return SetupHourlyData(model);
		}

		private DrillDown SetupHourlyData(DrillDown model)
		{
			ReportSeries data = new ReportSeries(model.SelectedSensor.MinimumValue);
			data.name = GetName(model.SelectedSensor);
			data.data = GetHourlyData(ref model, model.SelectedSensor.SensorId);
			model.Unit = model.SelectedSensor.UnitName;
			model.SelectedSensor = new Sensor()
			{
				SensorId = model.SelectedSensor.SensorId,
				MinimumValue = model.SelectedSensor.MinimumValue
			};
			model.Series.Add(data);
			return model;
		}

		private DrillDown GeneratetSeriesDataWeekly(DrillDown model)
		{
			SetGraphTitleAndSubTitle(ref model, "Weekly Data Review", "Data for Week no = " + model.Week);
			model.SelectedSensor = GetPumpSingleSensor(model.SelectedSensor.SensorId);
			return SetupWeeklyData(model);
		}

		private DrillDown SetupWeeklyData(DrillDown model)
		{
			ReportSeries data = new ReportSeries();
			data.name = GetName(model.SelectedSensor);
			data.data = GetWeeklyData(ref model);
			model.Series.Add(data);
			model.Unit = model.SelectedSensor.UnitName;
			model.SelectedSensor = new Sensor()
			{
				SensorId = model.SelectedSensor.SensorId,
				MinimumValue = model.SelectedSensor.MinimumValue
			};
			return model;
		}

		private DrillDown GeneratetSeriesDataMonthly(DrillDown model)
		{
			SetGraphTitleAndSubTitle(ref model, "Monthly Data Review", "Data for " + model.FromDateTime.ToString("MMM"));
			model.SelectedSensor = GetPumpSingleSensor(model.SelectedSensor.SensorId);
			return SetupMonthlyData(model);
		}

		private static void SetGraphTitleAndSubTitle(ref DrillDown model, string title, string subtitle)
		{
			model.GraphTitle = title;
			model.GraphSubTitle = subtitle;

		}

		private DrillDown SetupMonthlyData(DrillDown model)
		{
			ReportSeries data = new ReportSeries();
			data.name = GetName(model.SelectedSensor);
			data.data = GetMonthlyData(ref model);
			model.Series.Add(data);
			model.Unit = model.SelectedSensor.UnitName;
			model.SelectedSensor = new Sensor()
			{
				SensorId = model.SelectedSensor.SensorId,
				MinimumValue = model.SelectedSensor.MinimumValue
			};
			return model;
		}

		private T GetPumpStationSensor<T>(PumpStation pumpStation, ref DrillDown model) where T : Sensor
		{
			foreach (var sensor in pumpStation.Sensors)
			{
				if (sensor is PressureSensor && model.SelectedSensor.SensorId == sensor.SensorId)
				{
					Sensor p = new PressureSensor()
					{
						SensorId = sensor.SensorId,
						UUID = sensor.UUID,
						CurrentValue = sensor.CurrentValue,
						MinimumValue = sensor.MinimumValue
					};
					model.Unit = sensor.UnitName;
					return (T) p;
				}

				if (sensor is LevelSensor && model.TransmeType == Sensor.TransmitterType.LEVEL_TRANSMITTER)
				{
					Sensor p = new LevelSensor()
					{
						SensorId = sensor.SensorId,
						UUID = sensor.UUID,
						CurrentValue = sensor.CurrentValue,
						MinimumValue = sensor.MinimumValue
					};
					model.Unit = sensor.UnitName;
					return (T) p;
				}

				if (sensor is EnergySensor && model.TransmeType == Sensor.TransmitterType.ENERGY_TRANSMITTER)
				{
					Sensor p = new EnergySensor()
					{
						SensorId = sensor.SensorId,
						UUID = sensor.UUID,
						CurrentValue = sensor.CurrentValue,
						MinimumValue = sensor.MinimumValue,
						CumulativeValue = ((EnergySensor) sensor).CumulativeValue
					};
					model.Unit = sensor.UnitName;
					return (T) p;
				}

				if (sensor is FlowSensor && model.TransmeType == Sensor.TransmitterType.FLOW_TRANSMITTER)
				{
					Sensor p = new FlowSensor()
					{
						SensorId = sensor.SensorId,
						UUID = sensor.UUID,
						CurrentValue = sensor.CurrentValue,
						MinimumValue = sensor.MinimumValue,
						CumulativeValue = ((FlowSensor) sensor).CumulativeValue
					};
					model.Unit = sensor.UnitName;
					return (T) p;

				}
				if (sensor is ChlorinePresenceDetector && model.TransmeType == Sensor.TransmitterType.CHLORINE_PRESENCE_DETECTOR)
				{
					Sensor p = new ChlorinePresenceDetector()
					{
						SensorId = sensor.SensorId,
						UUID = sensor.UUID,
						CurrentValue = sensor.CurrentValue,
						MinimumValue = sensor.MinimumValue
					};
					model.Unit = sensor.UnitName;
					return (T) p;

				}

				if (sensor is ACPresenceDetector && model.TransmeType == Sensor.TransmitterType.AC_PRESENCE_DETECTOR)
				{
					Sensor p = new ACPresenceDetector()
					{
						SensorId = sensor.SensorId,
						UUID = sensor.UUID,
						CurrentValue = sensor.CurrentValue,
						MinimumValue = sensor.MinimumValue
					};
					model.Unit = sensor.UnitName;
					return (T) p;

				}

				if (sensor is BatteryVoltageDetector && model.TransmeType == Sensor.TransmitterType.BATTERY_VOLTAGE_DETECTOR)
				{
					Sensor p = new BatteryVoltageDetector()
					{
						SensorId = sensor.SensorId,
						UUID = sensor.UUID,
						CurrentValue = sensor.CurrentValue,
						MinimumValue = sensor.MinimumValue
					};
					model.Unit = sensor.UnitName;
					return (T) p;

				}
			}
			return (T) Activator.CreateInstance(typeof (T));

		}

		private T GetPumpStationSensor<T>(PumpStation pumpStation, ref UnderThresold model) where T : Sensor
		{
			foreach (var sensor in pumpStation.Sensors)
			{
				if (sensor is PressureSensor && model.TransmeType == Sensor.TransmitterType.PRESSURE_TRANSMITTER)
				{
					Sensor p = new PressureSensor()
					{
						SensorId = sensor.SensorId,
						UUID = sensor.UUID,
						CurrentValue = sensor.CurrentValue,
						MinimumValue = sensor.MinimumValue
					};
					model.Unit = sensor.UnitName;
					return (T) p;
				}

				if (sensor is LevelSensor && model.TransmeType == Sensor.TransmitterType.LEVEL_TRANSMITTER)
				{
					Sensor p = new LevelSensor()
					{
						SensorId = sensor.SensorId,
						UUID = sensor.UUID,
						CurrentValue = sensor.CurrentValue,
						MinimumValue = sensor.MinimumValue
					};
					model.Unit = sensor.UnitName;
					return (T) p;
				}

				if (sensor is EnergySensor && model.TransmeType == Sensor.TransmitterType.ENERGY_TRANSMITTER)
				{
					Sensor p = new EnergySensor()
					{
						SensorId = sensor.SensorId,
						UUID = sensor.UUID,
						CurrentValue = sensor.CurrentValue,
						MinimumValue = sensor.MinimumValue
					};
					model.Unit = sensor.UnitName;
					return (T) p;
				}

				if (sensor is FlowSensor && model.TransmeType == Sensor.TransmitterType.FLOW_TRANSMITTER)
				{
					Sensor p = new FlowSensor()
					{
						SensorId = sensor.SensorId,
						UUID = sensor.UUID,
						CurrentValue = sensor.CurrentValue,
						MinimumValue = sensor.MinimumValue
					};
					model.Unit = sensor.UnitName;
					return (T) p;

				}
				if (sensor is ChlorinePresenceDetector && model.TransmeType == Sensor.TransmitterType.CHLORINE_PRESENCE_DETECTOR)
				{
					Sensor p = new ChlorinePresenceDetector()
					{
						SensorId = sensor.SensorId,
						UUID = sensor.UUID,
						CurrentValue = sensor.CurrentValue,
						MinimumValue = sensor.MinimumValue
					};
					model.Unit = sensor.UnitName;
					return (T) p;

				}

				if (sensor is ACPresenceDetector && model.TransmeType == Sensor.TransmitterType.AC_PRESENCE_DETECTOR)
				{
					Sensor p = new ACPresenceDetector()
					{
						SensorId = sensor.SensorId,
						UUID = sensor.UUID,
						CurrentValue = sensor.CurrentValue,
						MinimumValue = sensor.MinimumValue
					};
					model.Unit = sensor.UnitName;
					return (T) p;

				}

				if (sensor is BatteryVoltageDetector && model.TransmeType == Sensor.TransmitterType.BATTERY_VOLTAGE_DETECTOR)
				{
					Sensor p = new BatteryVoltageDetector()
					{
						SensorId = sensor.SensorId,
						UUID = sensor.UUID,
						CurrentValue = sensor.CurrentValue,
						MinimumValue = sensor.MinimumValue
					};
					model.Unit = sensor.UnitName;
					return (T) p;

				}
			}
			return (T) Activator.CreateInstance(typeof (T));

		}
    

    private List<double> GetDailyData(ref DrillDown model)
		{
			model.XaxisCategory = new string[25];
			List<double> avgValue = new List<double>();
			for (int i = 0; i < 24; i++)
			{

                DateTime calDate = new DateTime(model.FromDateTime.Year, model.FromDateTime.Month, model.FromDateTime.Day, i, 0, 0);
				avgValue.Add(GetHourlyAverageValue(model.SelectedSensor.SensorId, calDate));
				model.XaxisCategory[i] = calDate.ToString();
			}
			return avgValue;
		}

		private List<double> GetUnderThresoldData(ref DrillDown model, int SensorId)
		{
			List<double> avgValue = new List<double>();
			for (int i = 0; i <= 24; i++)
			{
				if (model.TransmeType != Sensor.TransmitterType.ENERGY_TRANSMITTER)
				{
					avgValue.Add(GetTotalDataWithinTime(SensorId, model.FromDateTime.AddHours(i),
						model.FromDateTime.AddHours(i + 1)));
					model.XaxisCategory[i] = model.FromDateTime.AddHours(i + 1).ToString();
				}
			}
			return avgValue;
		}

		private List<double> GetHourlyData(ref DrillDown model, int SensorId)
		{
			List<double> avgValue = new List<double>();
            //int upperLimit = 4;
            IList<SensorData>  datas = GetCurrentDataWithinTime(SensorId, model.FromDateTime, model.EndDateTime);
            model.XaxisCategory = new string[datas.Count];
            int i = 0;
            foreach (SensorData data in datas)
            {
                avgValue.Add((double)data.Value);
                model.XaxisCategory[i] = data.LoggedAt.ToShortTimeString();
                i++;
            }
   //         for (int i = 0; i < 12; i++)
			//{
			//	double value = (double) GetCurrentDataWithinTime(SensorId,
			//		model.FromDateTime.AddMinutes(i*5),
			//		model.FromDateTime.AddMinutes(upperLimit + i*5));
   //             avgValue.Add(value);
			//	model.XaxisCategory[i] = model.FromDateTime.AddMinutes(i*5).ToShortTimeString();

			//}
			return avgValue;
		}

		private List<double> GetWeeklyData(ref DrillDown model)
		{
			model.XaxisCategory = new string[7];
			List<double> avgValue = new List<double>();
			for (int i = 0; i < 7; i++)
			{

				avgValue.Add(GetDailyAverageValue(model.SelectedSensor.SensorId, model.FromDateTime.AddDays(i)));
				model.XaxisCategory[i] = model.FromDateTime.AddDays(i + 1).ToShortDateString();
			}
			return avgValue;
		}

		private List<double> GetMonthlyData(ref DrillDown model)
		{
			model.XaxisCategory = new string[30];
			List<double> avgValue = new List<double>();
			for (int i = 0; i < 30; i++)
			{
				avgValue.Add(GetDailyAverageValue(model.SelectedSensor.SensorId, model.FromDateTime.AddDays(i)));
				model.XaxisCategory[i] = model.FromDateTime.AddDays(i).ToShortDateString();
			}
			return avgValue;
		}

		private double GetTotalDataWithinTime(int SensorId, DateTime from, DateTime to)
		{

			//using (SDLUnitOfWork uow = new SDLUnitOfWork())
			//{
			//    SDLRepository repo = new SDLRepository(uow.CurrentObjectContext);
			//    List<SensorData> sensorDataList = repo.GetSensorDataListWithinTime(SensorId, from, to);

			//    if (sensorDataList != null)
			//        return (double)sensorDataList.Sum(x => x.Value);
			//    else
			return 0;
			//}

		}

		private double GetHourlyAverageValue(int SensorId, DateTime to)
		{
			SensorHourlySummaryData sensorData = null;
			try
			{
				sensorData = Container.Instances<SensorHourlySummaryData>()
					.Where(
						x =>
							(x.Sensor.SensorId == SensorId && x.ProcessAt.Year == to.Year &&
							 x.ProcessAt.Month == to.Month && x.ProcessAt.Day == to.Day && x.ProcessAt.Hour == to.Hour))
					.SingleOrDefault();

			}
			catch (Exception ex)
			{
			}


			if (sensorData != null)
				return (double) sensorData.DataValue;
			else
				return 0;

		}

		private double GetDailyAverageValue(int SensorId, DateTime to)
		{
			SensorDailySummaryData sensorData = null;
			try
			{
				//sensorData = Container.Instances<SensorDailySummaryData>().FirstOrDefault();
				sensorData = Container.Instances<SensorDailySummaryData>()
					.Where(
						x =>
							(x.Sensor.SensorId == SensorId && x.ProcessAt.Year == to.Year &&
							 x.ProcessAt.Month == to.Month && x.ProcessAt.Day == to.Day)).SingleOrDefault();

			}
			catch (Exception ex)
			{
			}
			if (sensorData != null)
				return (double) sensorData.DataValue;
			else
				return 0;

		}

		private IList<SensorData> GetCurrentDataWithinTime (int SensorId, DateTime from, DateTime to)
		{
			using (SDLUnitOfWork uow = new SDLUnitOfWork())
			{
				SDLRepository repo = new SDLRepository(uow.CurrentObjectContext);
				return repo.GetSensorDataWithinTime(SensorId, from, to);
			}
		}

		private List<UnderThresoldData> GetCurrentDataListWithinTime(Sensor sensor, DateTime from, DateTime to)
		{
			{

				List<UnderThresoldData> sensorDataList = GetSensorDataListWithinTime(sensor.SensorId, from, to, sensor.MinimumValue);

				return sensorDataList;
			}
		}

		public List<UnderThresoldData> GetSensorDataListWithinTime(int SensorId, DateTime from, DateTime to,
			decimal minimumValue)
		{
			List<UnderThresoldData> sensorDataList = Container.Instances<UnderThresoldData>()
				.Where(x => (x.Sensor.SensorId == SensorId && x.LoggedAt >= from && x.LoggedAt <= to)).ToList();


			return sensorDataList;

		}

    public MotorOnOff GenerateMotorOnOffData(MotorOnOff model)
    {

      var pumpStation =
        Container.Instances<PumpStation>().First(w => w.AreaId == model.SelectedPumpStationId);
      model.MotorOnOffDatas = Container.Instances<MotorOnOffSummaryData>()
        .Where(x => (x.Motor.MotorID == pumpStation.PumpMotors.MotorID && x.ProcessAt >= model.FromDateTime &&
                     x.ProcessAt <= model.EndDateTime)).ToList();
      return model;
    }
    #endregion
  }
}
