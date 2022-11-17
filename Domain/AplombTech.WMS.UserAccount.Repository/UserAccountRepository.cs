using AplombTech.WMS.Domain.Features;
using AplombTech.WMS.Domain.Repositories;
using AplombTech.WMS.Domain.UserAccounts;
using AplombTech.WMS.Utility;
using NakedObjects;
using NakedObjects.Menu;
using NakedObjects.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.UserAccount.Repository
{
    [DisplayName("User Accounts")]
    public class UserAccountRepository : AbstractFactoryAndRepository
    {
        #region Injected Services
        public LoggedInUserInfoDomainRepository LoggedInUserInfoDomainRepository { set; protected get; }
        #endregion
        public static void Menu(IMenu menu)
        {
            menu.CreateSubMenu("Users")
                .AddAction("AddUser")
                .AddAction("ShowAllUsers");

            menu.CreateSubMenu("Role")
                .AddAction("AddRole")
                .AddAction("ShowAllRoles");
            //menu.CreateSubMenu("FeatureType")
            //    .AddAction("AddFeatureType")
            //    .AddAction("ShowAllFeatureTypes");
            //menu.CreateSubMenu("Feature")
            //    .AddAction("AddFeature");
        }

        #region ASP DOT NET MEMBERSHIP
        #region ROLE
        public Role AddRole(string name)
        {
            Role role = Container.NewTransientInstance<Role>();

            role.Id = Guid.NewGuid().ToString();
            role.Name = name;

            Container.Persist(ref role);

            return role;
        }
        public string Validate0AddRole(string name)
        {
            Role role = (from r in Container.Instances<Role>()
                         where r.Name.ToLower() == name.ToLower()
                         select r).FirstOrDefault();

            if (role != null)
            {
                return "Duplicate Role";
            }

            return null;
        }
        public bool HideAddRole()
        {
            IList<Feature> features = LoggedInUserInfoDomainRepository.GetFeatureListByLoginUser();

            Feature feature =
                features.Where(w => w.FeatureCode == (int)Feature.UserAccountsFeatureEnums.AddRole
                && w.FeatureType.FeatureTypeName == FeatureType.FeatureTypeEnums.UserAccount.ToString()).FirstOrDefault();

            if (feature == null)
                return true;
            return false;
        }
        [Eagerly(EagerlyAttribute.Do.Rendering)]
        [TableView(true, "Name")]
        public IQueryable<Role> ShowAllRoles()
        {
            return Container.Instances<Role>();
        }
        public bool HideShowAllRoles()
        {
            IList<Feature> features = LoggedInUserInfoDomainRepository.GetFeatureListByLoginUser();

            Feature feature =
                features.Where(w => w.FeatureCode == (int)Feature.UserAccountsFeatureEnums.ShowAllRoles
                && w.FeatureType.FeatureTypeName == FeatureType.FeatureTypeEnums.UserAccount.ToString()).FirstOrDefault();

            if (feature == null)
                return true;
            return false;
        }
        #endregion

        #region USERS
        public LoginUser AddUser([RegEx(Validation = @"^[\-\w\.]+@[\-\w\.]+\.[A-Za-z]+$", Message = "Not a valid email address")]string email, [DataType(DataType.Password)]string password, [DataType(DataType.Password)]string confirmPassword)
        {
            LoginUser user = Container.NewTransientInstance<LoginUser>();

            user.Id = Guid.NewGuid().ToString();
            user.UserName = email;
            user.Email = email;
            user.EmailConfirmed = false;
            user.PasswordHash = PasswordHash.HashPassword(password);
            user.SecurityStamp = Guid.NewGuid().ToString();
            user.PhoneNumberConfirmed = false;
            user.TwoFactorEnabled = false;
            user.LockoutEnabled = false;
            user.AccessFailedCount = 0;

            Container.Persist(ref user);

            return user;
        }
        public bool HideAddUser()
        {
            IList<Feature> features = LoggedInUserInfoDomainRepository.GetFeatureListByLoginUser();

            Feature feature =
                features.Where(w => w.FeatureCode == (int)Feature.UserAccountsFeatureEnums.AddLoginUser
                && w.FeatureType.FeatureTypeName == FeatureType.FeatureTypeEnums.UserAccount.ToString()).FirstOrDefault();

            if (feature == null)
                return true;
            return false;
        }
        public string ValidateAddUser(string email, string password, string confirmPassword)
        {
            LoginUser user = (from u in Container.Instances<LoginUser>()
                              where u.Email.ToLower() == email.ToLower()
                              select u).FirstOrDefault();

            if (user != null)
            {
                return "Duplicate User Name/Email";
            }
            if (password != confirmPassword)
            {
                return "Password does not match";
            }

            return null;
        }

        [Eagerly(EagerlyAttribute.Do.Rendering)]
        [TableView(true, "Email", "Role")]
        public IQueryable<LoginUser> ShowAllUsers()
        {
            return Container.Instances<LoginUser>();
        }
        public bool HideShowAllUsers()
        {
            IList<Feature> features = LoggedInUserInfoDomainRepository.GetFeatureListByLoginUser();

            Feature feature =
                features.Where(w => w.FeatureCode == (int)Feature.UserAccountsFeatureEnums.ShowAllUsers
                && w.FeatureType.FeatureTypeName == FeatureType.FeatureTypeEnums.UserAccount.ToString()).FirstOrDefault();

            if (feature == null)
                return true;
            return false;
        }
        #endregion

        #region Feature Type
        //public void AddFeatureType(string typeName)
        //{
        //    FeatureType feature = Container.NewTransientInstance<FeatureType>();

        //    feature.FeatureTypeName = typeName;

        //    Container.Persist(ref feature);
        //}
        //public bool HideAddFeatureType()
        //{
        //    IList<Feature> features = LoggedInUserInfoDomainRepository.GetFeatureListByLoginUser();

        //    Feature feature =
        //        features.Where(w => w.FeatureCode == (int)Feature.UserAccountsFeatureEnums.AddFeatureType
        //        && w.FeatureType.FeatureTypeName == FeatureType.FeatureTypeEnums.UserAccount.ToString()).FirstOrDefault();

        //    if (feature == null)
        //        return true;
        //    return false;
        //}
        //[Eagerly(EagerlyAttribute.Do.Rendering)]
        //[TableView(true, "FeatureTypeName")]
        //public IQueryable<FeatureType> ShowAllFeatureTypes()
        //{
        //    return Container.Instances<FeatureType>();
        //}
        //public bool HideShowAllFeatureTypes()
        //{
        //    IList<Feature> features = LoggedInUserInfoDomainRepository.GetFeatureListByLoginUser();

        //    Feature feature =
        //        features.Where(w => w.FeatureCode == (int)Feature.UserAccountsFeatureEnums.ShowAllFeatureTypes
        //        && w.FeatureType.FeatureTypeName == FeatureType.FeatureTypeEnums.UserAccount.ToString()).FirstOrDefault();

        //    if (feature == null)
        //        return true;
        //    return false;
        //}
        #endregion

        #region Feature
        //public void AddFeature(string featureName, FeatureType featureType)
        //{
        //    Feature feature = Container.NewTransientInstance<Feature>();

        //    feature.FeatureName = featureName;
        //    feature.FeatureType = featureType;

        //    Container.Persist(ref feature);
        //}
        //public bool HideAddFeature()
        //{
        //    IList<Feature> features = LoggedInUserInfoDomainRepository.GetFeatureListByLoginUser();

        //    Feature feature =
        //        features.Where(w => w.FeatureCode == (int)Feature.UserAccountsFeatureEnums.AddFeature
        //        && w.FeatureType.FeatureTypeName == FeatureType.FeatureTypeEnums.UserAccount.ToString()).FirstOrDefault();

        //    if (feature == null)
        //        return true;
        //    return false;
        //}
        #endregion
        #endregion
    }
}
