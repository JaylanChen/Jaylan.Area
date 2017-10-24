using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using NBSArea.EntityFramework.Entities;

namespace NBSArea.EntityFramework
{
    public partial class AreaDbContext : DbContext
    {
        public virtual DbSet<NBSRegion> NBSRegion { get; set; }
        public virtual DbSet<Entities.NBSArea> NBSArea { get; set; }


        public AreaDbContext()
            : base("name=AreaDbContext")
        {
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<NBSRegion>().ToTable("NBSRegion").HasKey(c => c.Id);

            modelBuilder.Entity<NBSRegion>().Property(c => c.Name).IsRequired().HasMaxLength(50).IsUnicode().IsRequired();
            modelBuilder.Entity<NBSRegion>().Property(c => c.Code).HasMaxLength(20).IsUnicode(false).IsRequired();
            modelBuilder.Entity<NBSRegion>().Property(c => c.ParentCode).HasMaxLength(10).IsUnicode(false).IsRequired();
            modelBuilder.Entity<NBSRegion>().Property(e => e.ZipCode).HasMaxLength(10).IsUnicode(false).IsOptional();

            modelBuilder.Entity<NBSRegion>().Property(c => c.CreationTime).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);




            modelBuilder.Entity<Entities.NBSArea>().ToTable("NBSArea").HasKey(c => c.Id);

            modelBuilder.Entity<Entities.NBSArea>().Property(c => c.Name).IsRequired().HasMaxLength(50).IsUnicode().IsRequired();
            modelBuilder.Entity<Entities.NBSArea>().Property(c => c.Code).HasMaxLength(20).IsUnicode(false).IsRequired();
            modelBuilder.Entity<Entities.NBSArea>().Property(c => c.ParentCode).HasMaxLength(10).IsUnicode(false).IsRequired();
            modelBuilder.Entity<Entities.NBSArea>().Property(c => c.ChildNodeUrl).HasMaxLength(512).IsUnicode(false);

            modelBuilder.Entity<Entities.NBSArea>().Property(c => c.CreationTime).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
}
