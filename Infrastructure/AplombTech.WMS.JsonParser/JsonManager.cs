using AplombTech.WMS.JsonParser.Entity;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.WMS.JsonParser.DeviceMessages;
using AplombTech.WMS.Domain.Motors;
using AplombTech.WMS.Domain.Sensors;
using AplombTech.WMS.Domain.Devices;
using Newtonsoft.Json;

namespace AplombTech.WMS.JsonParser
{
	public class JsonManager
	{
		public static DateTime GetSensorLoggedAtTime(string message)
		{
			JObject o = JObject.Parse(message);

			string loggedAt = o["PumpStation"]["LogDateTime"].ToString();

			DateTime loggesAtTime = Convert.ToDateTime(loggedAt, CultureInfo.CreateSpecificCulture("en-US"));
			return loggesAtTime;
		}

		public static bool GetPumpStationSensorDataComplete(string message)
		{
			JObject o = JObject.Parse(message);

			string sensorDataComplete = o["PumpStation"]["SensorDataComplete"].ToString();

			bool dataComplete = Convert.ToBoolean(sensorDataComplete, CultureInfo.CreateSpecificCulture("en-US"));
			return dataComplete;
		}

		public static int GetSensorDataLogCount(string message)
		{
			JObject o = JObject.Parse(message);

			string logCount = o["PumpStation"]["logCnt"].ToString();

			int count = Convert.ToInt32(logCount);
			return count;
		}

		public static DateTime GetConfigurationLoggedAtTime(string message)
		{
			JObject o = JObject.Parse(message);

			string loggedAt = o["PumpStation"]["ConfigureDateTime"].ToString();
			DateTime loggesAtTime = Convert.ToDateTime(loggedAt, CultureInfo.CreateSpecificCulture("en-US"));
			return loggesAtTime;
		}

		public static string GetPumpStationIDFromJson(string message)
		{
			JObject o = JObject.Parse(message);

			string pumpStationId = o["PumpStation"]["PumpStation_Id"].ToString();
			//int stationId = Convert.ToInt32(pumpStationId);
			return pumpStationId;
		}

		public static SensorMessage GetSensorObject(string message)
		{
			SensorMessage sensorObject = new SensorMessage();
			try
			{
				JObject o = JObject.Parse(message);
				//sensorObject.PumpStationId = GetPumpStationIDFromJson(message);
				sensorObject.LoggedAt = GetSensorLoggedAtTime(message);

        if (!string.IsNullOrEmpty((o["Sensors"]).ToString()))
				{
          Action<SensorValue> AddSensorValue = x => { if (x != null) sensorObject.Sensors.Add(x); };
          var JSensorObject = (JObject)o["Sensors"];
          foreach (JProperty prop in JSensorObject.Properties())
          {
            var propName = prop.Name;//ACP,BV,CPD,ET,FT,LT,PT,WM,WM
            var specialSensor = o["Sensors"][propName];
            SensorValue tSensor=null;
            if (specialSensor is JObject)
            {
              tSensor = GetSensorData(propName, o);
            }
            else if (specialSensor is JArray)
            { 
              if (propName.Equals("FT"))
              {
                tSensor = GetFlowSensorData(propName, o, 0);
              }
              else if (propName.Equals("ET"))
              {
                tSensor = GetEnergySensorData(propName, o, 0);
              }
              else
              {
                tSensor = GetSensorData(propName, o, 0);
              }
            }
            AddSensorValue(tSensor);
          }
        }
				
        if (!string.IsNullOrEmpty((o["Motors"]).ToString()))
				{
					sensorObject.Motors.Add(GetMotorData("Pump_Motor", o, 0));
          sensorObject.Motors.Add(GetMotorData("Chlorine_Motor", o, 0));
        }

				
				return sensorObject;
			}
			catch (Exception ex)
			{
				return null;
			}
		}
		public static VfdMessage GetVfdObject (string message)
		{
			VfdMessage vfdObject = new VfdMessage();
			try
			{
				JObject o = JObject.Parse(message);
				//vfdObject.PumpStationId = GetPumpStationIDFromJson(message);
				vfdObject.LoggedAt = GetSensorLoggedAtTime(message);
				vfdObject.VfdDataComplete = GetVfdDataComplete(message);
				vfdObject.LogCount = GetVfdLogCount(message);

				if (!string.IsNullOrEmpty((o["VFD"]).ToString()))
				{
					vfdObject.VfdList.Add(GetVfdData(o));
				}
			
				return vfdObject;
			}
			catch (Exception)
			{
				return null;
			}
		}
		public static int GetVfdDataComplete (string message)
		{
			JObject o = JObject.Parse(message);

			string loggedAt = o["PumpStation"]["VFDDataComplete"].ToString();

			int loggesAtTime = Convert.ToInt32(loggedAt, CultureInfo.CreateSpecificCulture("en-US"));
			return loggesAtTime;
		}
		public static int GetVfdLogCount (string message)
		{
			JObject o = JObject.Parse(message);

			string strlogCount = o["PumpStation"]["logCnt"].ToString();

			int logCount = Convert.ToInt32(strlogCount, CultureInfo.CreateSpecificCulture("en-US"));
			return logCount;
		}
		private static VfdValue GetVfdData (JObject o)
		{
			VfdValue data = new VfdValue();
			data.VfdUid = (string)o["VFD"][0]["uid"];
			data.Current = (decimal)o["VFD"][0]["Current"];
			data.Energy = (decimal)o["VFD"][0]["Energy"];
			data.Frequency = (decimal)o["VFD"][0]["Freq"];
			data.Power = (decimal)o["VFD"][0]["Power"];
			data.Voltage = (decimal)o["VFD"][0]["Voltage"];
			data.OperatingHour = (decimal)o["VFD"][0]["OperHr"];
			data.RunningHour = (decimal)o["VFD"][0]["RunHr"];

			return data;
		}

    private static FlowSensorValue GetFlowSensorData(string root, JObject o, int index)
    {
      try
      {
        FlowSensorValue data = new FlowSensorValue();
        data.SensorUUID = (string)o["Sensors"][root][index]["uid"];
        data.Value = (string)o["Sensors"][root][index]["value"];
        data.FT_Rate = (string)o["Sensors"][root][index]["ft_rate"];
        data.SecondaryValue  = (string)o["Sensors"][root][index]["ft_rate"];
        return data;
      }
      catch (Exception ex)
      {
        return null;
      }
    }
    private static EnergySensorValue GetEnergySensorData(string root, JObject o, int index)
    {
      try
      {
        var data = new EnergySensorValue
        {
          SensorUUID = (string) o["Sensors"][root][index]["uid"],
          Value = (string) o["Sensors"][root][index]["value"]
        };
        try
        {
          data.KhPerHourValue = (string) o["Sensors"][root][index]["power"];
        }
        catch (Exception)
        {
          data.KhPerHourValue = "0";
        }

        return data;
      }
      catch (Exception)
      {
        return null;
      }
    }
    private static SensorValue GetSensorData(string root, JObject o)
    {
      try
      {
        SensorValue data = new SensorValue();
        data.SensorUUID = (string)o["Sensors"][root]["uid"];
        data.Value = (string)o["Sensors"][root]["value"];
        return data;
      }
      catch (Exception ex)
      {
        return null;
      }
    }
    private static SensorValue GetSensorData(string root, JObject o, int index)
    {
      try
      {
        SensorValue data = new SensorValue();
        data.SensorUUID = (string)o["Sensors"][root][index]["uid"];
        data.Value = (string)o["Sensors"][root][index]["value"];
        return data;
      }
      catch (Exception ex)
      {
        return null;
      }
    }

    private static MotorValue GetMotorData(string root, JObject o, int index)
		{
			MotorValue data = new MotorValue();
			data.MotorUid = (string) o["Motors"][root][index]["uid"];
			data.Auto = string.IsNullOrEmpty((string) o["Motors"][root][index]["Auto"])
				? false
				: (bool) o["Motors"][root][index]["Auto"];
			data.MotorStatus = (string) o["Motors"][root][index]["Motor_Status"];
			data.LastCommand = (string) o["Motors"][root][index]["Last_Command"];
			data.LastCommandTime = (string) o["Motors"][root][index]["Last_Command_Time"];

			return data;
		}
	}
}
