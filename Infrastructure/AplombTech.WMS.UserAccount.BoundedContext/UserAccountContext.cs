using AplombTech.WMS.DataLayer;
using AplombTech.WMS.Domain.Features;
using AplombTech.WMS.Domain.UserAccounts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.UserAccount.BoundedContext
{
    public class UserAccountContext : BaseContext<UserAccountContext>
    {
        public DbSet<Role> Roles { get; set; }
        public DbSet<LoginUser> LoginUsers { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        public DbSet<UserLogins> UserLogins { get; set; }
        public DbSet<UserClaims> UserClaims { get; set; }
        public DbSet<FeatureType> FeatureTypes { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<RoleFeatures> RoleFeatures { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    Database.SetInitializer<UserAccountContext>(null);
        //}
    }
}
