namespace AplombTech.WMS.CompanyDatabase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sensors",
                c => new
                    {
                        SensorId = c.Int(nullable: false, identity: true),
                        UUID = c.String(maxLength: 250),
                        MinimumValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MaximumValue = c.Double(nullable: false),
                        CurrentValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LastDataReceived = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                        DataType = c.Int(nullable: false),
                        UnitName = c.String(),
                        Model = c.String(),
                        Version = c.String(),
                        Name = c.String(),
                        InsertedBy = c.String(nullable: false, maxLength: 50),
                        InsertedDate = c.DateTime(nullable: false),
                        LastUpdatedBy = c.String(nullable: false, maxLength: 50),
                        LastUpdatedDate = c.DateTime(nullable: false),
                        PumpStation_AreaId = c.Int(),
                    })
                .PrimaryKey(t => t.SensorId)
                .ForeignKey("dbo.Areas", t => t.PumpStation_AreaId)
                .Index(t => t.PumpStation_AreaId);
            
            CreateTable(
                "dbo.Areas",
                c => new
                    {
                        AreaId = c.Int(nullable: false, identity: true),
                        UUID = c.String(maxLength: 250),
                        Name = c.String(maxLength: 50),
                        Location = c.String(),
                        InsertedBy = c.String(nullable: false, maxLength: 50),
                        InsertedDate = c.DateTime(nullable: false),
                        LastUpdatedBy = c.String(nullable: false, maxLength: 50),
                        LastUpdatedDate = c.DateTime(nullable: false),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Address_AddressID = c.Int(),
                        Parent_AreaId = c.Int(),
                    })
                .PrimaryKey(t => t.AreaId)
                .ForeignKey("dbo.Addresses", t => t.Address_AddressID)
                .ForeignKey("dbo.Areas", t => t.Parent_AreaId)
                .Index(t => t.Address_AddressID)
                .Index(t => t.Parent_AreaId);
            
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        AddressID = c.Int(nullable: false, identity: true),
                        Street1 = c.String(maxLength: 200),
                        Street2 = c.String(maxLength: 200),
                        ZipCode = c.String(maxLength: 50),
                        Zone = c.String(maxLength: 50),
                        City = c.String(maxLength: 50),
                        InsertedBy = c.String(nullable: false, maxLength: 50),
                        InsertedDate = c.DateTime(nullable: false),
                        LastUpdatedBy = c.String(nullable: false, maxLength: 50),
                        LastUpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.AddressID);
            
            CreateTable(
                "dbo.AlertLogs",
                c => new
                    {
                        AlertLogId = c.Int(nullable: false, identity: true),
                        ReciverEmail = c.String(nullable: false, maxLength: 50),
                        ReciverMobileNo = c.String(nullable: false, maxLength: 50),
                        AlertMessageType = c.Int(nullable: false),
                        MessageDateTime = c.DateTime(nullable: false),
                        AlertGereratedObjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AlertLogId);
            
            CreateTable(
                "dbo.AlertRecipients",
                c => new
                    {
                        ReceipientId = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        MobileNo = c.String(nullable: false),
                        Email = c.String(),
                        InsertedBy = c.String(nullable: false, maxLength: 50),
                        InsertedDate = c.DateTime(nullable: false),
                        LastUpdatedBy = c.String(nullable: false, maxLength: 50),
                        LastUpdatedDate = c.DateTime(nullable: false),
                        Designation_DesignationId = c.Int(),
                    })
                .PrimaryKey(t => t.ReceipientId)
                .ForeignKey("dbo.Designations", t => t.Designation_DesignationId)
                .Index(t => t.Designation_DesignationId);
            
            CreateTable(
                "dbo.AlertTypes",
                c => new
                    {
                        AlertTypeId = c.Int(nullable: false, identity: true),
                        AlertName = c.String(maxLength: 50),
                        AlertMessage = c.String(),
                        InsertedBy = c.String(nullable: false, maxLength: 50),
                        InsertedDate = c.DateTime(nullable: false),
                        LastUpdatedBy = c.String(nullable: false, maxLength: 50),
                        LastUpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.AlertTypeId);
            
            CreateTable(
                "dbo.Designations",
                c => new
                    {
                        DesignationId = c.Int(nullable: false, identity: true),
                        DesignationShortName = c.String(maxLength: 50),
                        DesignationName = c.String(maxLength: 50),
                        InsertedBy = c.String(nullable: false, maxLength: 50),
                        InsertedDate = c.DateTime(nullable: false),
                        LastUpdatedBy = c.String(nullable: false, maxLength: 50),
                        LastUpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.DesignationId);
            
            CreateTable(
                "dbo.Devices",
                c => new
                    {
                        DeviceID = c.Int(nullable: false, identity: true),
                        UUID = c.String(maxLength: 20),
                        InsertedBy = c.String(nullable: false, maxLength: 50),
                        InsertedDate = c.DateTime(nullable: false),
                        LastUpdatedBy = c.String(nullable: false, maxLength: 50),
                        LastUpdatedDate = c.DateTime(nullable: false),
                        PumpStation_AreaId = c.Int(),
                    })
                .PrimaryKey(t => t.DeviceID)
                .ForeignKey("dbo.Areas", t => t.PumpStation_AreaId)
                .Index(t => t.PumpStation_AreaId);
            
            CreateTable(
                "dbo.Motors",
                c => new
                    {
                        MotorID = c.Int(nullable: false, identity: true),
                        UUID = c.String(nullable: false, maxLength: 250),
                        Auto = c.Boolean(nullable: false),
                        Controllable = c.Boolean(nullable: false),
                        MotorStatus = c.String(),
                        LastCommand = c.String(),
                        LastCommandTime = c.String(),
                        RemoveRemarks = c.String(),
                        IsRemoved = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        LastDataReceived = c.DateTime(),
                        InsertedBy = c.String(nullable: false, maxLength: 50),
                        InsertedDate = c.DateTime(nullable: false),
                        LastUpdatedBy = c.String(nullable: false, maxLength: 50),
                        LastUpdatedDate = c.DateTime(nullable: false),
                        PumpStation_AreaId = c.Int(),
                    })
                .PrimaryKey(t => t.MotorID)
                .ForeignKey("dbo.Areas", t => t.PumpStation_AreaId)
                .Index(t => t.PumpStation_AreaId);
            
            CreateTable(
                "dbo.Features",
                c => new
                    {
                        FeatureId = c.Int(nullable: false, identity: true),
                        FeatureName = c.String(nullable: false, maxLength: 100),
                        FeatureCode = c.Int(nullable: false),
                        FeatureType_FeatureTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FeatureId)
                .ForeignKey("dbo.FeatureTypes", t => t.FeatureType_FeatureTypeId, cascadeDelete: true)
                .Index(t => t.FeatureType_FeatureTypeId);
            
            CreateTable(
                "dbo.FeatureTypes",
                c => new
                    {
                        FeatureTypeId = c.Int(nullable: false, identity: true),
                        FeatureTypeName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.FeatureTypeId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserName = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MotorOnOffSummaryData",
                c => new
                    {
                        OnOffDataId = c.Long(nullable: false, identity: true),
                        OffDateTime = c.DateTime(nullable: false),
                        OnDateTime = c.DateTime(),
                        Duration = c.Double(nullable: false),
                        ProcessAt = c.DateTime(nullable: false),
                        Motor_MotorID = c.Int(),
                    })
                .PrimaryKey(t => t.OnOffDataId)
                .ForeignKey("dbo.Motors", t => t.Motor_MotorID)
                .Index(t => t.Motor_MotorID);
            
            CreateTable(
                "dbo.RoleFeatures",
                c => new
                    {
                        FeatureId = c.Int(nullable: false),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.FeatureId, t.RoleId })
                .ForeignKey("dbo.Features", t => t.FeatureId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.FeatureId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SensorDailyAverageData",
                c => new
                    {
                        AverageDataId = c.Long(nullable: false, identity: true),
                        DataValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DataCount = c.Int(nullable: false),
                        AverageValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DataDate = c.DateTime(nullable: false),
                        ProcessAt = c.DateTime(nullable: false),
                        Sensor_SensorId = c.Int(),
                    })
                .PrimaryKey(t => t.AverageDataId)
                .ForeignKey("dbo.Sensors", t => t.Sensor_SensorId)
                .Index(t => t.Sensor_SensorId);
            
            CreateTable(
                "dbo.SensorDailySummaryData",
                c => new
                    {
                        SummaryDataId = c.Long(nullable: false, identity: true),
                        ReceivedValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DataValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DataDate = c.DateTime(nullable: false),
                        ProcessAt = c.DateTime(nullable: false),
                        Sensor_SensorId = c.Int(),
                    })
                .PrimaryKey(t => t.SummaryDataId)
                .ForeignKey("dbo.Sensors", t => t.Sensor_SensorId)
                .Index(t => t.Sensor_SensorId);
            
            CreateTable(
                "dbo.SensorHourlyAverageData",
                c => new
                    {
                        AverageDataId = c.Long(nullable: false, identity: true),
                        DataHour = c.Int(nullable: false),
                        DataValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DataCount = c.Int(nullable: false),
                        AverageValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DataDate = c.DateTime(nullable: false),
                        ProcessAt = c.DateTime(nullable: false),
                        Sensor_SensorId = c.Int(),
                    })
                .PrimaryKey(t => t.AverageDataId)
                .ForeignKey("dbo.Sensors", t => t.Sensor_SensorId)
                .Index(t => t.Sensor_SensorId);
            
            CreateTable(
                "dbo.SensorHourlySummaryData",
                c => new
                    {
                        SummaryDataId = c.Long(nullable: false, identity: true),
                        DataHour = c.Int(nullable: false),
                        ReceivedValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DataValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DataDate = c.DateTime(nullable: false),
                        ProcessAt = c.DateTime(nullable: false),
                        Sensor_SensorId = c.Int(),
                    })
                .PrimaryKey(t => t.SummaryDataId)
                .ForeignKey("dbo.Sensors", t => t.Sensor_SensorId)
                .Index(t => t.Sensor_SensorId);
            
            CreateTable(
                "dbo.SensorMinutelySummaryData",
                c => new
                    {
                        SummaryDataId = c.Long(nullable: false, identity: true),
                        ReceivedValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DataValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DataDate = c.DateTime(nullable: false),
                        ProcessAt = c.DateTime(nullable: false),
                        Sensor_SensorId = c.Int(),
                    })
                .PrimaryKey(t => t.SummaryDataId)
                .ForeignKey("dbo.Sensors", t => t.Sensor_SensorId)
                .Index(t => t.Sensor_SensorId);
            
            CreateTable(
                "dbo.SensorOnOffSummaryData",
                c => new
                    {
                        OnOffDataId = c.Long(nullable: false, identity: true),
                        OffDateTime = c.DateTime(nullable: false),
                        OnDateTime = c.DateTime(),
                        Duration = c.Double(nullable: false),
                        ProcessAt = c.DateTime(nullable: false),
                        Sensor_SensorId = c.Int(),
                    })
                .PrimaryKey(t => t.OnOffDataId)
                .ForeignKey("dbo.Sensors", t => t.Sensor_SensorId)
                .Index(t => t.Sensor_SensorId);
            
            CreateTable(
                "dbo.UnderThresoldDatas",
                c => new
                    {
                        UnderThresoldDataId = c.Long(nullable: false, identity: true),
                        TransmeType = c.String(),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Unit = c.String(),
                        LoggedAt = c.DateTime(nullable: false),
                        Sensor_SensorId = c.Int(),
                    })
                .PrimaryKey(t => t.UnderThresoldDataId)
                .ForeignKey("dbo.Sensors", t => t.Sensor_SensorId)
                .Index(t => t.Sensor_SensorId);
            
            CreateTable(
                "dbo.Units",
                c => new
                    {
                        UnitID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        InsertedBy = c.String(nullable: false, maxLength: 50),
                        InsertedDate = c.DateTime(nullable: false),
                        LastUpdatedBy = c.String(nullable: false, maxLength: 50),
                        LastUpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UnitID);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.VariableFrequencyDrives",
                c => new
                    {
                        VfdId = c.Int(nullable: false, identity: true),
                        UUID = c.String(nullable: false, maxLength: 50),
                        Current = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Energy = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Frequency = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Power = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Voltage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OperatingHour = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RunningHour = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LastDataReceived = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                        Model = c.String(),
                        Version = c.String(),
                        InsertedBy = c.String(nullable: false, maxLength: 50),
                        InsertedDate = c.DateTime(nullable: false),
                        LastUpdatedBy = c.String(nullable: false, maxLength: 50),
                        LastUpdatedDate = c.DateTime(nullable: false),
                        PumpStation_AreaId = c.Int(),
                    })
                .PrimaryKey(t => t.VfdId)
                .ForeignKey("dbo.Areas", t => t.PumpStation_AreaId)
                .Index(t => t.PumpStation_AreaId);
            
            CreateTable(
                "dbo.AlertTypeAlertRecipients",
                c => new
                    {
                        AlertType_AlertTypeId = c.Int(nullable: false),
                        AlertRecipient_ReceipientId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AlertType_AlertTypeId, t.AlertRecipient_ReceipientId })
                .ForeignKey("dbo.AlertTypes", t => t.AlertType_AlertTypeId, cascadeDelete: true)
                .ForeignKey("dbo.AlertRecipients", t => t.AlertRecipient_ReceipientId, cascadeDelete: true)
                .Index(t => t.AlertType_AlertTypeId)
                .Index(t => t.AlertRecipient_ReceipientId);
            
            CreateTable(
                "dbo.ACPresenceDetectors",
                c => new
                    {
                        SensorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SensorId)
                .ForeignKey("dbo.Sensors", t => t.SensorId)
                .Index(t => t.SensorId);
            
            CreateTable(
                "dbo.BatteryVoltageDetectors",
                c => new
                    {
                        SensorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SensorId)
                .ForeignKey("dbo.Sensors", t => t.SensorId)
                .Index(t => t.SensorId);
            
            CreateTable(
                "dbo.Cameras",
                c => new
                    {
                        DeviceID = c.Int(nullable: false),
                        URL = c.String(maxLength: 150),
                    })
                .PrimaryKey(t => t.DeviceID)
                .ForeignKey("dbo.Devices", t => t.DeviceID)
                .Index(t => t.DeviceID);
            
            CreateTable(
                "dbo.ChlorineMotors",
                c => new
                    {
                        MotorID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MotorID)
                .ForeignKey("dbo.Motors", t => t.MotorID)
                .Index(t => t.MotorID);
            
            CreateTable(
                "dbo.ChlorinePresenceDetectors",
                c => new
                    {
                        SensorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SensorId)
                .ForeignKey("dbo.Sensors", t => t.SensorId)
                .Index(t => t.SensorId);
            
            CreateTable(
                "dbo.EnergySensors",
                c => new
                    {
                        SensorId = c.Int(nullable: false),
                        CumulativeValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        KwPerHourValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.SensorId)
                .ForeignKey("dbo.Sensors", t => t.SensorId)
                .Index(t => t.SensorId);
            
            CreateTable(
                "dbo.FlowSensors",
                c => new
                    {
                        SensorId = c.Int(nullable: false),
                        CumulativeValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LitrePerMinuteValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MeterCubePerHourValue = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.SensorId)
                .ForeignKey("dbo.Sensors", t => t.SensorId)
                .Index(t => t.SensorId);
            
            CreateTable(
                "dbo.LevelSensors",
                c => new
                    {
                        SensorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SensorId)
                .ForeignKey("dbo.Sensors", t => t.SensorId)
                .Index(t => t.SensorId);
            
            CreateTable(
                "dbo.PressureSensors",
                c => new
                    {
                        SensorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SensorId)
                .ForeignKey("dbo.Sensors", t => t.SensorId)
                .Index(t => t.SensorId);
            
            CreateTable(
                "dbo.PumpMotors",
                c => new
                    {
                        MotorID = c.Int(nullable: false),
                        ModelNo = c.String(maxLength: 50),
                        Capacity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StaticWaterLevel = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MotorID)
                .ForeignKey("dbo.Motors", t => t.MotorID)
                .Index(t => t.MotorID);
            
            CreateTable(
                "dbo.Routers",
                c => new
                    {
                        DeviceID = c.Int(nullable: false),
                        MACAddress = c.String(maxLength: 50),
                        IP = c.String(maxLength: 20),
                        Port = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DeviceID)
                .ForeignKey("dbo.Devices", t => t.DeviceID)
                .Index(t => t.DeviceID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Routers", "DeviceID", "dbo.Devices");
            DropForeignKey("dbo.PumpMotors", "MotorID", "dbo.Motors");
            DropForeignKey("dbo.PressureSensors", "SensorId", "dbo.Sensors");
            DropForeignKey("dbo.LevelSensors", "SensorId", "dbo.Sensors");
            DropForeignKey("dbo.FlowSensors", "SensorId", "dbo.Sensors");
            DropForeignKey("dbo.EnergySensors", "SensorId", "dbo.Sensors");
            DropForeignKey("dbo.ChlorinePresenceDetectors", "SensorId", "dbo.Sensors");
            DropForeignKey("dbo.ChlorineMotors", "MotorID", "dbo.Motors");
            DropForeignKey("dbo.Cameras", "DeviceID", "dbo.Devices");
            DropForeignKey("dbo.BatteryVoltageDetectors", "SensorId", "dbo.Sensors");
            DropForeignKey("dbo.ACPresenceDetectors", "SensorId", "dbo.Sensors");
            DropForeignKey("dbo.VariableFrequencyDrives", "PumpStation_AreaId", "dbo.Areas");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UnderThresoldDatas", "Sensor_SensorId", "dbo.Sensors");
            DropForeignKey("dbo.SensorOnOffSummaryData", "Sensor_SensorId", "dbo.Sensors");
            DropForeignKey("dbo.SensorMinutelySummaryData", "Sensor_SensorId", "dbo.Sensors");
            DropForeignKey("dbo.SensorHourlySummaryData", "Sensor_SensorId", "dbo.Sensors");
            DropForeignKey("dbo.SensorHourlyAverageData", "Sensor_SensorId", "dbo.Sensors");
            DropForeignKey("dbo.SensorDailySummaryData", "Sensor_SensorId", "dbo.Sensors");
            DropForeignKey("dbo.SensorDailyAverageData", "Sensor_SensorId", "dbo.Sensors");
            DropForeignKey("dbo.Sensors", "PumpStation_AreaId", "dbo.Areas");
            DropForeignKey("dbo.RoleFeatures", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.RoleFeatures", "FeatureId", "dbo.Features");
            DropForeignKey("dbo.MotorOnOffSummaryData", "Motor_MotorID", "dbo.Motors");
            DropForeignKey("dbo.Motors", "PumpStation_AreaId", "dbo.Areas");
            DropForeignKey("dbo.Features", "FeatureType_FeatureTypeId", "dbo.FeatureTypes");
            DropForeignKey("dbo.Devices", "PumpStation_AreaId", "dbo.Areas");
            DropForeignKey("dbo.AlertRecipients", "Designation_DesignationId", "dbo.Designations");
            DropForeignKey("dbo.AlertTypeAlertRecipients", "AlertRecipient_ReceipientId", "dbo.AlertRecipients");
            DropForeignKey("dbo.AlertTypeAlertRecipients", "AlertType_AlertTypeId", "dbo.AlertTypes");
            DropForeignKey("dbo.Areas", "Parent_AreaId", "dbo.Areas");
            DropForeignKey("dbo.Areas", "Address_AddressID", "dbo.Addresses");
            DropIndex("dbo.Routers", new[] { "DeviceID" });
            DropIndex("dbo.PumpMotors", new[] { "MotorID" });
            DropIndex("dbo.PressureSensors", new[] { "SensorId" });
            DropIndex("dbo.LevelSensors", new[] { "SensorId" });
            DropIndex("dbo.FlowSensors", new[] { "SensorId" });
            DropIndex("dbo.EnergySensors", new[] { "SensorId" });
            DropIndex("dbo.ChlorinePresenceDetectors", new[] { "SensorId" });
            DropIndex("dbo.ChlorineMotors", new[] { "MotorID" });
            DropIndex("dbo.Cameras", new[] { "DeviceID" });
            DropIndex("dbo.BatteryVoltageDetectors", new[] { "SensorId" });
            DropIndex("dbo.ACPresenceDetectors", new[] { "SensorId" });
            DropIndex("dbo.AlertTypeAlertRecipients", new[] { "AlertRecipient_ReceipientId" });
            DropIndex("dbo.AlertTypeAlertRecipients", new[] { "AlertType_AlertTypeId" });
            DropIndex("dbo.VariableFrequencyDrives", new[] { "PumpStation_AreaId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.UnderThresoldDatas", new[] { "Sensor_SensorId" });
            DropIndex("dbo.SensorOnOffSummaryData", new[] { "Sensor_SensorId" });
            DropIndex("dbo.SensorMinutelySummaryData", new[] { "Sensor_SensorId" });
            DropIndex("dbo.SensorHourlySummaryData", new[] { "Sensor_SensorId" });
            DropIndex("dbo.SensorHourlyAverageData", new[] { "Sensor_SensorId" });
            DropIndex("dbo.SensorDailySummaryData", new[] { "Sensor_SensorId" });
            DropIndex("dbo.SensorDailyAverageData", new[] { "Sensor_SensorId" });
            DropIndex("dbo.RoleFeatures", new[] { "RoleId" });
            DropIndex("dbo.RoleFeatures", new[] { "FeatureId" });
            DropIndex("dbo.MotorOnOffSummaryData", new[] { "Motor_MotorID" });
            DropIndex("dbo.Features", new[] { "FeatureType_FeatureTypeId" });
            DropIndex("dbo.Motors", new[] { "PumpStation_AreaId" });
            DropIndex("dbo.Devices", new[] { "PumpStation_AreaId" });
            DropIndex("dbo.AlertRecipients", new[] { "Designation_DesignationId" });
            DropIndex("dbo.Areas", new[] { "Parent_AreaId" });
            DropIndex("dbo.Areas", new[] { "Address_AddressID" });
            DropIndex("dbo.Sensors", new[] { "PumpStation_AreaId" });
            DropTable("dbo.Routers");
            DropTable("dbo.PumpMotors");
            DropTable("dbo.PressureSensors");
            DropTable("dbo.LevelSensors");
            DropTable("dbo.FlowSensors");
            DropTable("dbo.EnergySensors");
            DropTable("dbo.ChlorinePresenceDetectors");
            DropTable("dbo.ChlorineMotors");
            DropTable("dbo.Cameras");
            DropTable("dbo.BatteryVoltageDetectors");
            DropTable("dbo.ACPresenceDetectors");
            DropTable("dbo.AlertTypeAlertRecipients");
            DropTable("dbo.VariableFrequencyDrives");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.Units");
            DropTable("dbo.UnderThresoldDatas");
            DropTable("dbo.SensorOnOffSummaryData");
            DropTable("dbo.SensorMinutelySummaryData");
            DropTable("dbo.SensorHourlySummaryData");
            DropTable("dbo.SensorHourlyAverageData");
            DropTable("dbo.SensorDailySummaryData");
            DropTable("dbo.SensorDailyAverageData");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.RoleFeatures");
            DropTable("dbo.MotorOnOffSummaryData");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.FeatureTypes");
            DropTable("dbo.Features");
            DropTable("dbo.Motors");
            DropTable("dbo.Devices");
            DropTable("dbo.Designations");
            DropTable("dbo.AlertTypes");
            DropTable("dbo.AlertRecipients");
            DropTable("dbo.AlertLogs");
            DropTable("dbo.Addresses");
            DropTable("dbo.Areas");
            DropTable("dbo.Sensors");
        }
    }
}
