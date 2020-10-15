using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace CheeryAssessment.Models
{
    public class DB_Entities : DbContext
    {
        public DB_Entities() : base("CheeryDB") { }
        public DbSet<UserModel> userModels { get; set; }
        public DbSet<UserBalanceModel> userBalanceModels { get; set; }
        public DbSet<ExchangeLogModel> exchangeLogModels { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>()
                .ToTable("tbl_user")
                .HasOptional(e => e.UserBalanceModel)
                .WithRequired(a => a.UserModel);
            
            modelBuilder.Entity<UserBalanceModel>()
                .ToTable("tbl_user_balance")
                .HasKey(e => e.user_id);

            modelBuilder.Entity<ExchangeLogModel>()
                .ToTable("tbl_exchange_log")
                .HasRequired<UserBalanceModel>(b => b.UserBalanceModel)
                .WithMany(a => a.ExchangeLogModels)
                .HasForeignKey<long>(b => b.user_id);

            
            modelBuilder.Conventions.Remove<PluralizingEntitySetNameConvention>();

            base.OnModelCreating(modelBuilder);
        }
    }
}