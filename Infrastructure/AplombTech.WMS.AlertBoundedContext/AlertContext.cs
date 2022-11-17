using AplombTech.WMS.DataLayer;
using AplombTech.WMS.Domain.Alerts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.AlertBoundedContext
{
    public class AlertContext :BaseContext<AlertContext>
    {
        public DbSet<AlertType> AlertTypes { get; set; }
        public DbSet<Designation> Designations { get; set; }
        public DbSet<AlertRecipient> AlertRecipients { get; set; }
        public DbSet<AlertLog> AlertLogs { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    Database.SetInitializer<AlertContext>(null);
        //}
    }
}
