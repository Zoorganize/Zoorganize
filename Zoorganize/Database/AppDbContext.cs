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
        public DbSet<Animal> Animals { get; set; }
        public DbSet<VeterinaryAppointment> VeterinaryAppointments { get; set; }
        public DbSet<ExternalZooStay> ExternalZooStays { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Animal>().HasMany(a => a.VeterinaryAppointments)
                .WithOne(va => va.Animal)
                .HasForeignKey(va => va.AnimalId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Animal>().HasMany(a => a.ExternalZooStays)
                .WithOne(ezs => ezs.Animal)
                .HasForeignKey(ezs => ezs.AnimalId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite(@"Data Source=Database/Zoorganize.db");

        
    }
}
