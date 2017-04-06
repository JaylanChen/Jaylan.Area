using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using MiArea.EntityFramework.Entities;

namespace MiArea.EntityFramework
{
    public partial class AreaDbContext : DbContext
    {
        public virtual DbSet<MiCity> MiCity { get; set; }


        public AreaDbContext()
            : base("name=AreaDbContext")
        {
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<MiCity>().ToTable("MiCity").HasKey(c => c.Id);

            modelBuilder.Entity<MiCity>().Property(c => c.Name).IsRequired().HasMaxLength(50).IsUnicode();
            modelBuilder.Entity<MiCity>().Property(c => c.ZipCode).HasMaxLength(10).IsUnicode(false);
            modelBuilder.Entity<MiCity>().Property(c => c.CreationTime).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
}
