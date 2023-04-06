using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CMS.Core.Domain;
using Microsoft.Extensions.Configuration;
using CMS.Core.Helper;

namespace CMS.Data.EFCore
{
    public class CrawlDB : DbContext
    {
        public CrawlDB()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESSKHANG;Database=CrawlDB;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Series>()
                .HasOne<Comic>(s => s.Comic)
                .WithMany(g => g.Series)
                .HasForeignKey(s => s.ComicId)
                .IsRequired();
        }

        public DbSet<Comic> Comics { get; set; }
        public DbSet<Series> Series { get; set; }

    }
}