using NakedObjects;
using NakedObjects.Menu;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Domain.UserAccounts
{
    [Table("AspNetUsers")]
    public class LoginUser
    {
        #region Injected Services
        public IDomainObjectContainer Container { set; protected get; }
        #endregion

        #region Primitive Properties
        [Key, NakedObjectsIgnore]
        public virtual string Id { get; set; }
        [Title, Required, NakedObjectsIgnore]
        [MemberOrder(10)]
        public virtual string UserName { get; set; }
        [Required, MemberOrder(20), Disabled]
        public virtual string Email { get; set; }
        [Required, NakedObjectsIgnore]
        public virtual bool EmailConfirmed { get; set; }
        [NakedObjectsIgnore]
        public virtual string PasswordHash { get; set; }
        [NakedObjectsIgnore]
        public virtual string SecurityStamp { get; set; }
        [NakedObjectsIgnore]
        public virtual string PhoneNumber { get; set; }
        [Required, NakedObjectsIgnore]
        public virtual bool PhoneNumberConfirmed { get; set; }
        [Required, NakedObjectsIgnore]
        public virtual bool TwoFactorEnabled { get; set; }
        [NakedObjectsIgnore]
        public virtual DateTime? LockoutEndDateUtc { get; set; }
        [Required, NakedObjectsIgnore]
        public virtual bool LockoutEnabled { get; set; }
        [Required, NakedObjectsIgnore]
        public virtual int AccessFailedCount { get; set; }
        #endregion

        #region Get Properties      
        [MemberOrder(50), NotMapped]
        [DisplayName("Role")]
        public Role Role
        {
            get
            {
                Role role = (from r in Container.Instances<UserRoles>()
                              where r.LoginUser.Id == this.Id
                              select r.Role).FirstOrDefault();
                return role;
            }
        }
        #endregion

        #region Behavior
        #region Change Password
        public void ChangePassword([DataType(DataType.Password)] string password,
            [DataType(DataType.Password)] string confirmPassword)
        {
            this.PasswordHash = AplombTech.WMS.Utility.PasswordHash.HashPassword(password);
        }
        public string ValidateChangePassword(string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                return "Password does not match";
            }
            return null;
        }
        #endregion
        #region Add Role
        public void AddRole(Role role)
        {
            UserRoles user = Container.NewTransientInstance<UserRoles>();

            user.Role = role;
            user.LoginUser = this;

            Container.Persist(ref user);
        }
        #endregion
        #endregion

        #region Menu
        public static void Menu(IMenu menu)
        {
            menu.AddAction("AddRole");
            menu.AddAction("ChangePassword");           
        }
        #endregion                
    }
}
