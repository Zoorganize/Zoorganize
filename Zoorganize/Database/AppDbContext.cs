using Microsoft.EntityFrameworkCore;
using Zoorganize.Database.Models;

namespace Zoorganize.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<Keeper> Keepers { get; set; }
        public DbSet<Animal> Animals { get; set; }
        public DbSet<VeterinaryAppointment> VeterinaryAppointments { get; set; }
        public DbSet<ExternalZooStay> ExternalZooStays { get; set; }
        public DbSet<Species> Species { get; set; }
        public DbSet<AnimalEnclosure> AnimalEnclosures { get; set; }

        //TODO: OnModelCreating for relationships plus migrations
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
