namespace AplombTech.WMS.CompanyDatabase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WaterMeterAdd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WaterMeterSensors",
                c => new
                    {
                        SensorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SensorId)
                .ForeignKey("dbo.Sensors", t => t.SensorId)
                .Index(t => t.SensorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WaterMeterSensors", "SensorId", "dbo.Sensors");
            DropIndex("dbo.WaterMeterSensors", new[] { "SensorId" });
            DropTable("dbo.WaterMeterSensors");
        }
    }
}
