using AplombTech.WMS.DataLayer;
using AplombTech.WMS.Domain.Motors;
using AplombTech.WMS.Domain.Sensors;
using AplombTech.WMS.Domain.SummaryData;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.DataProcessBoundedContext
{
    public class ProcessContext : BaseContext<ProcessContext>
    {
        //public DbSet<DataLog> SensorDataLogs { get; set; }
        //public DbSet<SensorData> SensorDatas { get; set; }
        //public DbSet<MotorData> MotorDatas { get; set; }
        public DbSet<SensorHourlySummaryData> SensorHourlySummaryData { get; set; }
        public DbSet<SensorMinutelySummaryData> SensorMinutelySummaryData { get; set; }
        public DbSet<SensorDailySummaryData> SensorDailySummaryData { get; set; }
        public DbSet<SensorHourlyAverageData> SensorHourlyAverageData { get; set; }
        public DbSet<SensorDailyAverageData> SensorDailyAverageData { get; set; }
        public DbSet<SensorOnOffSummaryData> SensorOnOffSummaryData { get; set; }
        public DbSet<MotorOnOffSummaryData> MotorOnOffSummaryData { get; set; }
    }
}
