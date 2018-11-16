using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace Jaylan.Area.Data
{
    public class AreaDbContext : DbContext
    {
        /// <summary>
        /// NBS Area
        /// </summary>
        public DbSet<NBS_Area> NBS_Area { get; set; }

        /// <summary>
        /// Taobao Area
        /// </summary>
        public DbSet<Taobao_Area> Taobao_Area { get; set; }

        /// <summary>
        /// MI Area
        /// </summary>
        public DbSet<Mi_Area> Mi_Area { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var currentPath = Environment.CurrentDirectory;
            const string projectName = "Jaylan.Area";
            var projectIndex = currentPath.IndexOf(projectName, StringComparison.OrdinalIgnoreCase);
            var dbPath = Path.Combine(currentPath.Substring(0, projectIndex), projectName +"\\Jaylan.Area.Data\\DataBase\\Area.db");
            optionsBuilder.UseSqlite("Data Source=" + dbPath);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<NBS_Area>().ToTable("NBS_Area").HasKey(c => c.Id);
            modelBuilder.Entity<NBS_Area>().Property(c => c.Name).IsRequired().HasMaxLength(64).IsUnicode().IsRequired();
            modelBuilder.Entity<NBS_Area>().Property(c => c.Code).HasMaxLength(32).IsUnicode(false).IsRequired();
            modelBuilder.Entity<NBS_Area>().Property(c => c.ParentCode).HasMaxLength(32).IsUnicode(false).IsRequired();
            modelBuilder.Entity<NBS_Area>().Property(e => e.ZipCode).HasMaxLength(16).IsUnicode(false).IsRequired(false);
            modelBuilder.Entity<NBS_Area>().Property(c => c.ChildNodeUrl).HasMaxLength(512).IsUnicode(false);


            modelBuilder.Entity<Taobao_Area>().ToTable("Taobao_Area").HasKey(c => c.Id);
            modelBuilder.Entity<Taobao_Area>().Property(c => c.Name).IsRequired().HasMaxLength(64).IsUnicode().IsRequired();
            modelBuilder.Entity<Taobao_Area>().Property(c => c.Code).HasMaxLength(32).IsUnicode(false).IsRequired();
            modelBuilder.Entity<Taobao_Area>().Property(c => c.ParentCode).HasMaxLength(32).IsUnicode(false).IsRequired();
            modelBuilder.Entity<Taobao_Area>().Property(e => e.ZipCode).HasMaxLength(16).IsUnicode(false).IsRequired(false);

            
            modelBuilder.Entity<Mi_Area>().ToTable("Mi_Area").HasKey(c => c.Id);
            modelBuilder.Entity<Mi_Area>().Property(c => c.Name).IsRequired().HasMaxLength(64).IsUnicode().IsRequired();
            modelBuilder.Entity<Mi_Area>().Property(c => c.Code).HasMaxLength(32).IsUnicode(false).IsRequired();
            modelBuilder.Entity<Mi_Area>().Property(c => c.ParentCode).HasMaxLength(32).IsUnicode(false).IsRequired();
            modelBuilder.Entity<Mi_Area>().Property(e => e.ZipCode).HasMaxLength(16).IsUnicode(false).IsRequired(false);
        }
    }

}
