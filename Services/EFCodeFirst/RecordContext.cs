using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using DataEntitys;
using Services.Migrations;
using SQLite.CodeFirst;
using static Common.UtilityHelper;
namespace Services.EFCodeFirst
{
    public class RecordContext : DbContext
    {
        public RecordContext()
          : base($"name={(GetConfigurationKeyValue("IsUsedSqlite") == "1" ? GetConfigurationKeyValue("SqliteName") : GetConfigurationKeyValue("SqlserverName"))}")
        {
            //this.Configuration.AutoDetectChangesEnabled = false;
            // 判断是否已经进行了Migrate操作
            var isInitializer = GetConfigurationKeyValue("IsInitializer");
            if (isInitializer == "0")
            {
                if (GetConfigurationKeyValue("IsUsedSqlite") == "1")
                {
                    Database.SetInitializer(new MigrateDatabaseToLatestVersion<RecordContext, SqliteConfiguration>());
                }
                else
                {
                    Database.SetInitializer(new MigrateDatabaseToLatestVersion<RecordContext, SqlserverConfiguration>());
                }
                SetConfigurationKeyValue("IsInitializer", "1");
            }
        }

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
            if (Common.UtilityHelper.GetConfigurationKeyValue("IsUsedSqlite") == "0")
            {
                modelBuilder.Entity<DevelopUser>().Property(m => m.RowVersion).HasColumnType("Timestamp").IsRowVersion().IsConcurrencyToken();
                modelBuilder.Entity<DevelopRecord>().Property(m => m.RowVersion).HasColumnType("Timestamp").IsRowVersion().IsConcurrencyToken();
                modelBuilder.Entity<DevelopType>().Property(m => m.RowVersion).HasColumnType("Timestamp").IsRowVersion().IsConcurrencyToken();
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
