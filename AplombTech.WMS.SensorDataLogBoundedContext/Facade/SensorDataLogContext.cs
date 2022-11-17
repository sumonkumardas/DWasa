using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.WMS.Domain.Motors;
using AplombTech.WMS.Domain.Sensors;
using AplombTech.WMS.Domain.SummaryData;
using AplombTech.WMS.Domain.Vfds;

namespace AplombTech.WMS.SensorDataLogBoundedContext.Facade
{
	public class SensorDataLogContext : DbContext
	{
		public SensorDataLogContext() : base("SensorDataLogDatabase")
		{
		}

		public DbSet<DataLog> DataLogs { get; set; }
		public DbSet<SensorData> SensorDatas { get; set; }
		public DbSet<MotorData> MotorDatas { get; set; }
		public DbSet<VfdData> VfdDatas { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
#if DEBUG
			Database.SetInitializer<SensorDataLogContext>(null);
			//Database.SetInitializer(new CreateDatabaseIfNotExists<SensorDataLogContext>());
#else
            Database.SetInitializer<SensorDataLogContext>(null);
#endif

		}

	}
}
