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
            optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS02;Database=CrawlDB;Trusted_Connection=True;");
        }

        public DbSet<Comic> Comics { get; set; }
    }
}