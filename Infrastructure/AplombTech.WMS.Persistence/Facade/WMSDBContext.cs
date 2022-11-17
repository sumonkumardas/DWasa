using AplombTech.WMS.Domain.Alerts;
using AplombTech.WMS.Domain.Areas;
using AplombTech.WMS.Domain.Devices;
using AplombTech.WMS.Domain.Motors;
using AplombTech.WMS.Domain.Sensors;
using AplombTech.WMS.Domain.Shared;
using AplombTech.WMS.Domain.SummaryData;
using AplombTech.WMS.Domain.Vfds;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Persistence.Facade
{
  public class WMSDBContext : DbContext
  {
    //public WMSDBContext(string name) : base(name) { }
    public WMSDBContext() : base("CommandModelDatabase")
    {
    }

    //Add DbSet properties for root objects, thus:
    public DbSet<Area> Areas { get; set; }
    public DbSet<Device> Devices { get; set; }
    public DbSet<Camera> Cameras { get; set; }
    public DbSet<Router> Routers { get; set; }
    public DbSet<VariableFrequencyDrive> Vfds { get; set; }
    public DbSet<Motor> Motors { get; set; }
    public DbSet<PumpMotor> PumpMotors { get; set; }
    public DbSet<ChlorineMotor> ChlorineMotors { get; set; }
    public DbSet<Sensor> Sensors { get; set; }
    public DbSet<LevelSensor> LevelSensors { get; set; }
    public DbSet<FlowSensor> FlowSensors { get; set; }
    public DbSet<EnergySensor> EnergySensors { get; set; }
    public DbSet<ChlorinePresenceDetector> ChlorinePresenceDetectors { get; set; }
    public DbSet<BatteryVoltageDetector> BatteryVoltageDetectors { get; set; }
    public DbSet<PressureSensor> PressureSensors { get; set; }
    public DbSet<ACPresenceDetector> ACPresenceDetectors { get; set; }
    //public DbSet<SensorData> SensorDatas { get; set; }
    //public DbSet<MotorData> MotorDatas { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Unit> Units { get; set; }
    //public DbSet<DataLog> SensorDataLogs { get; set; }
    public DbSet<AlertType> AlertTypes { get; set; }
    public DbSet<Designation> Designations { get; set; }
    public DbSet<AlertRecipient> AlertRecipients { get; set; }
    public DbSet<AlertLog> AlertLogs { get; set; }
    public DbSet<SensorHourlySummaryData> SensorHourlySummaryData { get; set; }
    public DbSet<SensorDailySummaryData> SensorDailySummaryData { get; set; }
    public DbSet<SensorMinutelySummaryData> SensorMinutelySummaryData { get; set; }
    public DbSet<SensorHourlyAverageData> SensorHourlyAverageData { get; set; }
    public DbSet<SensorDailyAverageData> SensorDailyAverageData { get; set; }
    public DbSet<SensorOnOffSummaryData> SensorOnOffSummaryData { get; set; }
    public DbSet<MotorOnOffSummaryData> MotorOnOffSummaryData { get; set; }
    public DbSet<UnderThresoldData> UnderThresoldDatas { get; set; }
    public DbSet<PressureSensorLastThirtyData> PressureSensorLastThirtyDatas { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      //Initialisation
      //Use the Naked Objects > DbInitialiser template to add an initialiser, then reference thus:
      Database.SetInitializer<WMSDBContext>(null);

      //Mappings
      //Use the Naked Objects > DbMapping template to create mapping classes & reference them thus:
      //modelBuilder.Configurations.Add(new EmployeeMapping());
      //modelBuilder.Configurations.Add(new CustomerMapping());

      //modelBuilder.Entity<Asar>()
      //.Property(f => f.DateOfEstablishment)
      //.HasColumnType("datetime2");

      //modelBuilder.Properties<DateTime>()
      //.Configure(c => c.HasColumnType("datetime2"));

      modelBuilder.Entity<Sensor>().Property(sensor => sensor.MinimumValue).HasPrecision(18, 2);
      //modelBuilder.Entity<Sensor>().Property(sensor => sensor.MaximumValue).HasPrecision(18, 2);
      modelBuilder.Entity<Domain.Motors.PumpMotor>().Property(pump => pump.Capacity).HasPrecision(18, 2);
    }
  }
}
