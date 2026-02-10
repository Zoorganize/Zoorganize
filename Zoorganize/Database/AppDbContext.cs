using Microsoft.EntityFrameworkCore;
using Zoorganize.Database.Models;

namespace Zoorganize.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<Staff> Staff { get; set; }
        public DbSet<Animal> Animals { get; set; }
        public DbSet<VeterinaryAppointment> VeterinaryAppointments { get; set; }
        public DbSet<ExternalZooStay> ExternalZooStays { get; set; }
        public DbSet<Species> Species { get; set; }
        public DbSet<AnimalEnclosure> AnimalEnclosures { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Zoo> Zoos { get; set; }
        public DbSet<StaffRooms> StaffRooms { get; set; }
        public DbSet<VisitorRoom> VisitorRooms { get; set; }

        //TODO: OnModelCreating for relationships plus migrations
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // TPT Strategie aktivieren
            modelBuilder.Entity<Room>().UseTptMappingStrategy();

            modelBuilder.Entity<Animal>(entity =>
            {
                // Beziehung zu Species
                entity.HasOne(a => a.Species)
                    .WithMany()
                    .HasForeignKey(a => a.SpeciesId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Beziehung zu AnimalEnclosure
                entity.HasOne(a => a.CurrentEnclosure)
                    .WithMany(e => e.Animals)
                    .HasForeignKey(a => a.CurrentEnclosureId)
                    .OnDelete(DeleteBehavior.SetNull);

                // Beziehung zu VeterinaryAppointments
                entity.HasMany(a => a.VeterinaryAppointments)
                    .WithOne(va => va.Animal)
                    .HasForeignKey(va => va.AnimalId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Beziehung zu ExternalZooStays
                entity.HasMany(a => a.ExternalZooStays)
                    .WithOne(ezs => ezs.Animal)
                    .HasForeignKey(ezs => ezs.AnimalId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Many-to-Many Beziehung zu Staff (Keepers)
                entity.HasOne(a => a.Keeper)
                    .WithMany(s => s.AssignedAnimals)
                    .HasForeignKey(a => a.KeeperId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // AnimalEnclosure
            modelBuilder.Entity<AnimalEnclosure>(entity =>
            {
                entity.HasMany(e => e.AllowedSpecies)
                    .WithMany();
            });

            // Staff
            modelBuilder.Entity<Staff>(entity =>
            {
                entity.HasMany(s => s.AuthorizedSpecies)
                    .WithMany();
            });

            // StaffRooms
            modelBuilder.Entity<StaffRooms>(entity =>
            {
                entity.HasMany(sr => sr.AuthorizedStaff)
                    .WithMany();
            });

            // VisitorRoom
            modelBuilder.Entity<VisitorRoom>(entity =>
            {
                entity.HasMany(vr => vr.Staff)
                    .WithMany();
            });

        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var projectPath = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
            var dbPath = Path.Combine(projectPath, "Database", "Zoorganize.db");

            options.UseSqlite($"Data Source={dbPath}");
        }
            

        
    }
}
