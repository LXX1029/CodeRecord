using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataEntitys;
namespace Services.EFCodeFirst
{
    public class RecordContext : DbContext
    {
        public RecordContext() : base("name=RecordDBConnectionString")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<RecordContext, Migrations.Configuration>());
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
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Configurations.Add(new DevelopUserConfiguration());
            //modelBuilder.Configurations.Add(new DevelopTypeConfiguration());
            //modelBuilder.Configurations.Add(new DevelopPowerFunConfiguration());
        }

        public DbSet<DevelopUser> DevelopUsers { get; set; }
        public DbSet<DevelopType> DevelopTypes { get; set; }
        public DbSet<DevelopRecord> DevelopRecords { get; set; }
        public DbSet<DevelopFun> DevelopFuns { get; set; }
        public DbSet<DevelopPowerFun> DevelopPowerFuns { get; set; }
    }
}
