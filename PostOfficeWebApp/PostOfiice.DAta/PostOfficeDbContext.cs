﻿using Microsoft.AspNet.Identity.EntityFramework;
using PostOffice.Model.Models;
using System.Data.Entity;

namespace PostOfiice.DAta
{
    public class PostOfficeDbContext : IdentityDbContext<ApplicationUser>
    {
        public PostOfficeDbContext() : base("PostOfficeConnection")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<District> Districts { get; set; }

        public DbSet<PaymentMethod> PaymentMethods { get; set; }

        public DbSet<PO> PostOffices { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceGroup> ServiceGroups { get; set; }
        public DbSet<MainServiceGroup> MainServiceGroups { get; set; }

        public DbSet<Error> Errors { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionDetail> TransactionDetails { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }
        public DbSet<VisittorStatistic> VisittorStatistics { get; set; }
        public DbSet<ApplicationGroup> ApplicationGroups { set; get; }
        public DbSet<ApplicationRole> ApplicationRoles { set; get; }
        public DbSet<ApplicationRoleGroup> ApplicationRoleGroups { set; get; }
        public DbSet<ApplicationUserGroup> ApplicationUserGroups { set; get; }
        
        public DbSet<Percent> Percents { set; get; }
        public DbSet<PropertyService> PropertyServices { set; get; }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            builder.Entity<IdentityUserRole>().HasKey(i => new { i.UserId, i.RoleId }).ToTable("ApplicationUserRoles");
            builder.Entity<IdentityUserLogin>().HasKey(i => i.UserId).ToTable("ApplicationUserLogins");
            builder.Entity<IdentityRole>().ToTable("ApplicationRoles");
            builder.Entity<IdentityUserClaim>().HasKey(i => i.UserId).ToTable("ApplicationUserClaims");
            builder.Entity<PropertyService>().Property(x => x.Percent).HasPrecision(18, 10);
        }

        public static PostOfficeDbContext Create()
        {
            return new PostOfficeDbContext();
        }
    }
}