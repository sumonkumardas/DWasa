namespace AplombTech.WMS.CompanyDatabase.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
  using AplombTech.WMS.Domain.Alerts;
  using AplombTech.WMS.Domain.Features;
  using AplombTech.WMS.Domain.UserAccounts;
  using AplombTech.WMS.Utility;
  using System.Collections.Generic;
  using System.Text;
  using System.Threading.Tasks;

  internal sealed class Configuration : DbMigrationsConfiguration<AplombTech.WMS.CompanyDatabase.CompanyDatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
    protected override void Seed(CompanyDatabaseContext context) { }

    //protected override void Seed(CompanyDatabaseContext context)
    //{
    //  CreateDesignation("PO", "Pump Operator", context);
    //  CreateDesignation("SAE", "Sub Asst. Engineer", context);
    //  CreateDesignation("SDE/AE", "Sub Divisional Engineer/Asst. Engineer", context);
    //  CreateDesignation("EE", "Executive Engineer", context);
    //  CreateDesignation("CO", "Complain Operator", context);

    //  CreateAlertType("Pump On/Off", "of Pump Station|is Off. ", context);
    //  CreateAlertType("Under Threshold", "Under Threshold value of Sensor|of Pump Station ", context);
    //  CreateAlertType("Data Missing", " Data is missing of Sensor|of Pump Station ", context);

    //  CreateFeatureList(context);

    //  base.Seed(context);
    //}
    private void CreateDesignation(string shortName, string fullName, CompanyDatabaseContext context)
    {
      Designation desig = new Designation();
      desig.DesignationShortName = shortName;
      desig.DesignationName = fullName;

      desig.AuditFields.InsertedBy = "Automated";
      desig.AuditFields.InsertedDateTime = DateTime.Now;
      desig.AuditFields.LastUpdatedBy = "Automated";
      desig.AuditFields.LastUpdatedDateTime = DateTime.Now;
      context.Designations.Add(desig);
    }
    private void CreateAlertType(string alertName, string alertMessage, CompanyDatabaseContext context)
    {
      AlertType alert = new AlertType();
      alert.AlertName = alertName;
      alert.AlertMessage = alertMessage;

      alert.AuditFields.InsertedBy = "Automated";
      alert.AuditFields.InsertedDateTime = DateTime.Now;
      alert.AuditFields.LastUpdatedBy = "Automated";
      alert.AuditFields.LastUpdatedDateTime = DateTime.Now;
      context.AlertTypes.Add(alert);
    }
    private void CreateFeatureList(CompanyDatabaseContext context)
    {
      Role role = CreateRole("Admin", context);
      LoginUser adminUser = CreateAdminUser(role, context);
      AssignRoleToAdminUser(adminUser, role, context);

      FeatureType areaFeaturetype = CreateFeatureType(FeatureType.FeatureTypeEnums.Area.ToString(), context);
      AddAreaFeatures(areaFeaturetype, role, context);

      FeatureType alertFeaturetype = CreateFeatureType(FeatureType.FeatureTypeEnums.Alert.ToString(), context);
      AddAlertFeatures(alertFeaturetype, role, context);

      FeatureType userAccountsFeaturetype = CreateFeatureType(FeatureType.FeatureTypeEnums.UserAccount.ToString(), context);
      AddUserAccountsFeatures(userAccountsFeaturetype, role, context);

      FeatureType reportFeaturetype = CreateFeatureType(FeatureType.FeatureTypeEnums.Report.ToString(), context);
      AddUReportFeatures(reportFeaturetype, role, context);
    }
    private FeatureType CreateFeatureType(string typeName, CompanyDatabaseContext context)
    {
      FeatureType type = new FeatureType();
      type.FeatureTypeName = typeName;

      context.FeatureTypes.Add(type);

      return type;
    }
    private void AddAreaFeatures(FeatureType areaFeaturetype, Role role, CompanyDatabaseContext context)
    {
      var values = Enum.GetValues(typeof(Feature.AreaFeatureEnums));
      foreach (int value in values)
      {
        Feature feature = new Feature();
        feature.FeatureType = areaFeaturetype;
        feature.FeatureName = Enum.GetName(typeof(Feature.AreaFeatureEnums), value);
        feature.FeatureCode = value;

        context.Features.Add(feature);
        CreateRoleFeatures(feature, role, context);
      }
    }
    private void AddAlertFeatures(FeatureType alertFeaturetype, Role role, CompanyDatabaseContext context)
    {
      var values = Enum.GetValues(typeof(Feature.AlertFeatureEnums));
      foreach (int value in values)
      {
        Feature feature = new Feature();
        feature.FeatureType = alertFeaturetype;
        feature.FeatureName = Enum.GetName(typeof(Feature.AlertFeatureEnums), value);
        feature.FeatureCode = value;

        context.Features.Add(feature);
        CreateRoleFeatures(feature, role, context);
      }
    }
    private void AddUserAccountsFeatures(FeatureType userAccountsFeaturetype, Role role, CompanyDatabaseContext context)
    {
      var values = Enum.GetValues(typeof(Feature.UserAccountsFeatureEnums));
      foreach (int value in values)
      {
        Feature feature = new Feature();
        feature.FeatureType = userAccountsFeaturetype;
        feature.FeatureName = Enum.GetName(typeof(Feature.UserAccountsFeatureEnums), value);
        feature.FeatureCode = value;

        context.Features.Add(feature);
        CreateRoleFeatures(feature, role, context);
      }
    }
    private void AddUReportFeatures(FeatureType reportFeaturetype, Role role, CompanyDatabaseContext context)
    {
      var values = Enum.GetValues(typeof(Feature.ReportFeatureEnums));
      foreach (int value in values)
      {
        Feature feature = new Feature();
        feature.FeatureType = reportFeaturetype;
        feature.FeatureName = Enum.GetName(typeof(Feature.ReportFeatureEnums), value);
        feature.FeatureCode = value;

        context.Features.Add(feature);
        CreateRoleFeatures(feature, role, context);
      }
    }
    private LoginUser CreateAdminUser(Role role, CompanyDatabaseContext context)
    {
      LoginUser user = new LoginUser();
      user.Id = Guid.NewGuid().ToString();
      user.UserName = "admin@gmail.com";
      user.Email = "admin@gmail.com";
      user.EmailConfirmed = false;
      user.PasswordHash = PasswordHash.HashPassword("123456");
      user.SecurityStamp = Guid.NewGuid().ToString();
      user.PhoneNumberConfirmed = false;
      user.TwoFactorEnabled = false;
      user.LockoutEnabled = false;
      user.AccessFailedCount = 0;

      context.LoginUsers.Add(user);

      return user;
    }
    private Role CreateRole(string roleName, CompanyDatabaseContext context)
    {
      Role role = new Role();
      role.Id = Guid.NewGuid().ToString();
      role.Name = roleName;

      context.Roles.Add(role);

      return role;
    }
    private void AssignRoleToAdminUser(LoginUser adminUser, Role role, CompanyDatabaseContext context)
    {
      UserRoles userRole = new UserRoles();
      userRole.LoginUser = adminUser;
      userRole.Role = role;

      context.UserRoles.Add(userRole);
    }
    private void CreateRoleFeatures(Feature feature, Role role, CompanyDatabaseContext context)
    {
      RoleFeatures roleFeature = new RoleFeatures();
      roleFeature.Feature = feature;
      roleFeature.Role = role;

      context.RoleFeatures.Add(roleFeature);
    }
  }
}
