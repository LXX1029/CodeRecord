namespace Services.EFCodeFirst
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using DataEntitys;

    /// <summary>
    /// DevelopUser表配置
    /// </summary>
    public class DevelopUserConfiguration : EntityTypeConfiguration<DevelopUser>
    {
        public DevelopUserConfiguration()
        {
            this.ToTable("DevelopUsers");
            this.HasKey(m => m.Id).Property(m => m.Id).HasColumnType("int")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(m => m.Name).HasColumnType("nvarchar").HasMaxLength(15).IsRequired();
            this.Property(m => m.Pwd).HasColumnType("nvarchar").HasMaxLength(50).IsRequired();
            this.Property(m => m.Sex).HasColumnType("nchar").HasMaxLength(2);
            this.Property(m => m.DevelopAge).HasColumnType("decimal").HasPrecision(9, 2);
            //this.Property(m => m.RowVersion).IsConcurrencyToken(true);
        }
    }

    /// <summary>
    /// DevelopType表配置
    /// </summary>
    public class DevelopTypeConfiguration : EntityTypeConfiguration<DevelopType>
    {
        public DevelopTypeConfiguration()
        {
            this.ToTable("DevelopTypes");
            this.HasKey(m => m.Id).Property(m => m.Id).HasColumnType("int")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(m => m.Name).HasColumnType("nvarchar").HasMaxLength(50).IsRequired();
            this.Property(m => m.ParentId).HasColumnType("int").IsRequired();
            this.Property(m => m.CreatedTime).HasColumnType("datetime");
            this.Property(m => m.UpdatedTime).HasColumnType("datetime");
        }
    }

    /// <summary>
    /// DevelopPowerFun表配置
    /// </summary>
    public class DevelopPowerFunConfiguration : EntityTypeConfiguration<DevelopPowerFun>
    {
        public DevelopPowerFunConfiguration()
        {
            this.ToTable("DevelopPowerFuns");
            this.HasKey(m => m.Id).Property(m => m.Id).HasColumnType("int")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(m => m.IsEnabled).HasColumnType("bit").IsRequired();
            this.Property(m => m.UserId).HasColumnType("int").IsRequired();
            this.Property(m => m.FunId).HasColumnType("int").IsRequired();
        }
    }

    /// <summary>
    /// DevelopFun表配置
    /// </summary>
    public class DevelopFunConfiguration : EntityTypeConfiguration<DevelopFun>
    {
        public DevelopFunConfiguration()
        {
            this.ToTable("DevelopFuns");
            this.HasKey(m => m.Id).Property(m => m.Id).HasColumnType("int")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }

    /// <summary>
    /// DevelopFun表配置
    /// </summary>
    public class DevelopRecordConfiguration : EntityTypeConfiguration<DevelopRecord>
    {
        public DevelopRecordConfiguration()
        {
            this.ToTable("DevelopRecords");
            this.HasKey(m => m.Id).Property(m => m.Id).HasColumnType("int")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}
