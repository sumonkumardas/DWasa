using AplombTech.WMS.Domain.Areas;
using AplombTech.WMS.Domain.Devices;
using AplombTech.WMS.Domain.Features;
using AplombTech.WMS.Domain.Sensors;
using AplombTech.WMS.Domain.Shared;
using AplombTech.WMS.Domain.UserAccounts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.WMS.Domain.Motors;
using AplombTech.WMS.Domain.SummaryData;

namespace AplombTech.WMS.QueryModel.Facade
{
    public class QueryModelDatabase: DbContext
    {
        public QueryModelDatabase() { }
        public QueryModelDatabase(string name) : base(name)
        {
            //SensorDatas = base.Set<SensorData>();
           // MotorDatas = base.Set<MotorData>();
            SensorHourlySummaryData = base.Set<SensorHourlySummaryData>();
            SensorDailySummaryData = base.Set<SensorDailySummaryData>();
            SensorMinutelySummaryData = base.Set<SensorMinutelySummaryData>();
            SensorHourlyAverageData = base.Set<SensorHourlyAverageData>();
            SensorDailyAverageData = base.Set<SensorDailyAverageData>();
            SensorOnOffSummaryData = base.Set<SensorOnOffSummaryData>();
            MotorOnOffSummaryData = base.Set<MotorOnOffSummaryData>();
        }

        //public DbSet<SensorData> SensorDatas { get; private set; }
        //public DbSet<MotorData> MotorDatas { get; private set; }
        public DbSet<SensorHourlySummaryData> SensorHourlySummaryData { get; private set; }
        public DbSet<SensorDailySummaryData> SensorDailySummaryData { get; private set; }

        public DbSet<SensorMinutelySummaryData> SensorMinutelySummaryData { get; private set; }
        public DbSet<SensorHourlyAverageData> SensorHourlyAverageData { get; private set; }
        public DbSet<SensorDailyAverageData> SensorDailyAverageData { get; private set; }
        public DbSet<SensorOnOffSummaryData> SensorOnOffSummaryData { get; private set; }
        public DbSet<MotorOnOffSummaryData> MotorOnOffSummaryData { get; private set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Initialisation
            //Use the Naked Objects > DbInitialiser template to add an initialiser, then reference thus:
           
            //Database.SetInitializer<QueryModelDatabase>(null);
           
            modelBuilder.Ignore<Area>();
            //modelBuilder.Ignore<DMA>();
            modelBuilder.Ignore<PumpStation>();
            //modelBuilder.Ignore<Zone>();
            modelBuilder.Ignore<Sensor>();
            modelBuilder.Ignore<Motor>();
        }

        //public IQueryable<Zone> Zones
        //{
        //    get { return this._zones; }
        //}
        //public IQueryable<DMA> DMAs
        //{
        //    get { return _dmas; }
        //}
        //public IQueryable<PumpStation> PumpStations
        //{
        //    get { return _pumpStations; }
        //}
        //public IQueryable<Pump> Pumps
        //{
        //    get { return _pumps; }
        //}
        //public IQueryable<Camera> Cameras
        //{
        //    get { return _cameras; }
        //}
        //public IQueryable<Router> Routers
        //{
        //    get { return _routers; }
        //}
        //public IQueryable<Sensor> Sensors
        //{
        //    get { return _sensors; }
        //}
    }
}
