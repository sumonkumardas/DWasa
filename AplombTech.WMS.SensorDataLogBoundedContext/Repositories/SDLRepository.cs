using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.WMS.Domain.Motors;
using AplombTech.WMS.Domain.Sensors;
using AplombTech.WMS.Domain.SummaryData;
using AplombTech.WMS.Domain.Vfds;
using AplombTech.WMS.JsonParser.Entity;
using AplombTech.WMS.SensorDataLogBoundedContext.Facade;

namespace AplombTech.WMS.SensorDataLogBoundedContext.Repositories
{
	public class SDLRepository
	{
		private readonly SensorDataLogContext _sensorDataLogContext;

		public SDLRepository(SensorDataLogContext sensorDataLogContext)
		{
			_sensorDataLogContext = sensorDataLogContext;
		}

		public DataLog GetDataLog(string topic, DateTime loggedAtSensor, int stationId)
		{
			return
				_sensorDataLogContext.DataLogs.Where(
					w => w.PumpStationId == stationId && w.Topic == topic && w.LoggedAtSensor == loggedAtSensor)
					.FirstOrDefault();
		}

		public DataLog CreateDataLog(string topic, string message, DateTime loggedAtSensor, int stationId)
		{
			DataLog data = new DataLog();

			data.Topic = topic;
			data.Message = message;
			data.MessageReceivedAt = DateTime.Now;
			data.LoggedAtSensor = loggedAtSensor;
			data.ProcessingStatus = DataLog.ProcessingStatusEnum.None;
			data.PumpStationId = stationId;
			//_sensorDataLogContext.DataLogs.Add(data);
			return data;
		}

		public DataLog UpdateDataLog(long Id, DataLog.ProcessingStatusEnum status, string remarks)
		{
			DataLog data = _sensorDataLogContext.DataLogs.Where(x => x.SensorDataLogID == Id).SingleOrDefault();

			if (data != null)
			{
				data.ProcessingStatus = status;
				if (remarks != null)
					data.ExceptionMessage = remarks;

			}
			return data;
		}

		public SensorData CreateNewSensorData(SensorValue sensorData, DateTime loggedAt, Sensor sensor)
		{
			SensorData data = new SensorData();
			if (sensor.DataType == Sensor.Data_Type.Float)
				data.Value = Convert.ToDecimal(sensorData.Value);
			if (sensor.DataType == Sensor.Data_Type.Boolean)
			{
				if (sensorData.Value != null && sensorData.Value.Contains("."))
				{
					data.Value = Convert.ToDecimal(Convert.ToBoolean(Convert.ToDecimal(sensorData.Value)));
				}
				else
				{
					data.Value = Convert.ToDecimal(Convert.ToBoolean(Convert.ToDecimal(sensorData.Value)));
				}
			}
      if (sensor is FlowSensor)
      {
        data.Rate = Convert.ToDecimal(((FlowSensorValue)sensorData).FT_Rate);
      }
      data.LoggedAt = loggedAt;
			data.SensorId = sensor.SensorId;
			data.ProcessAt = DateTime.Now;

			_sensorDataLogContext.SensorDatas.Add(data);
			
			return data;
		}

		public IList<SensorData> GetSensorDataWithinTime (int SensorId, DateTime from, DateTime to)
		{
			IList<SensorData> sensorDatas = _sensorDataLogContext.SensorDatas
				.Where(x => (x.SensorId == SensorId && x.LoggedAt >= from && x.LoggedAt <= to)).OrderBy(o=>o.LoggedAt).ToList();

			return sensorDatas;
		}

		public SensorData GetSensorData(int SensorId)
		{
			SensorData sensorData = _sensorDataLogContext.SensorDatas
				.Where(x => (x.SensorId == SensorId)).OrderByDescending(o => o.ProcessAt).FirstOrDefault();

			return sensorData;

		}

		public void CreateNewMotorData(MotorData motorData)
		{
			_sensorDataLogContext.MotorDatas.Add(motorData);
		}

		public MotorData CreateNewMotorData (MotorValue data, DateTime loggedAt, Motor motor)
		{
			MotorData motorData = new MotorData();
			motorData.MotorStatus = data.MotorStatus;
			motorData.LastCommand = data.LastCommand;
			motorData.LastCommandTime = data.LastCommandTime;
			motorData.Auto = data.Auto;
			motorData.LoggedAt = loggedAt;
			motorData.ProcessAt = DateTime.Now;
			motorData.MotorId = motor.MotorID;
			_sensorDataLogContext.MotorDatas.Add(motorData);
			
			return motorData;
		}

		public void AddVfdData (VfdData vfdData)
		{
			_sensorDataLogContext.VfdDatas.Add(vfdData);
		}
	}
}
