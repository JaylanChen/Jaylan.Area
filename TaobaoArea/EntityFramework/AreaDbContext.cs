using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using TaobaoArea.EntityFramework.Entities;

namespace TaobaoArea.EntityFramework
{
    public partial class AreaDbContext : DbContext
    {
        public virtual DbSet<TaobaoRegion> TaobaoRegion { get; set; }


        public AreaDbContext()
            : base("name=AreaDbContext")
        {
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<TaobaoRegion>().ToTable("TaobaoRegion").HasKey(c => c.Id);

            modelBuilder.Entity<TaobaoRegion>().Property(c => c.Name).IsRequired().HasMaxLength(50).IsUnicode().IsRequired();
            modelBuilder.Entity<TaobaoRegion>().Property(c => c.Code).HasMaxLength(20).IsUnicode(false).IsRequired();
            modelBuilder.Entity<TaobaoRegion>().Property(c => c.ParentCode).HasMaxLength(10).IsUnicode(false).IsRequired();
            modelBuilder.Entity<TaobaoRegion>().Property(e => e.ZipCode).HasMaxLength(10).IsUnicode(false).IsOptional();

            modelBuilder.Entity<TaobaoRegion>().Property(c => c.CreationTime).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
}
