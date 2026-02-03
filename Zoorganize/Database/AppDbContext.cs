using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Zoorganize.Database.Models;

namespace Zoorganize.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<Pfleger> Pfleger { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite(@"Data Source=Database/Zoorganize.db");

        
    }
}
