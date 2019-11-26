using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using DataEntitys;
using Services.Migrations;
using SQLite.CodeFirst;

namespace Services.EFCodeFirst
{
    public class RecordContext : DbContext
    {
        public RecordContext()
          : base($"name={(Common.UtilityHelper.GetConfigurationKeyValue("IsUsedSqlite") == "1" ? "RecordDbSqlite" : "SqlServerConnectionString")}")
        { }

        public new Database Database
        {
            get
            {
                return base.Database;
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new DevelopUserConfiguration());
            modelBuilder.Configurations.Add(new DevelopTypeConfiguration());
            modelBuilder.Configurations.Add(new DevelopPowerFunConfiguration());
            modelBuilder.Configurations.Add(new DevelopFunConfiguration());
            modelBuilder.Configurations.Add(new DevelopRecordConfiguration());
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            if (Common.UtilityHelper.GetConfigurationKeyValue("IsUsedSqlite") == "1")
            {
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<RecordContext, SqliteConfiguration>());
            }
            else
            {
                modelBuilder.Entity<DevelopUser>().Property(m => m.RowVersion).HasColumnType("Timestamp").IsRowVersion().IsConcurrencyToken();
                modelBuilder.Entity<DevelopRecord>().Property(m => m.RowVersion).HasColumnType("Timestamp").IsRowVersion().IsConcurrencyToken();
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<RecordContext, Configuration>());
            }
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<DevelopUser> DevelopUsers { get; set; }
        public DbSet<DevelopType> DevelopTypes { get; set; }
        public DbSet<DevelopRecord> DevelopRecords { get; set; }
        public DbSet<DevelopFun> DevelopFuns { get; set; }
        public DbSet<DevelopPowerFun> DevelopPowerFuns { get; set; }
    }
}
