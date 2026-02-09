using Microsoft.EntityFrameworkCore;
using Zoorganize.Database;
using Zoorganize.Database.Models;
using Zoorganize.Models.Api;

namespace Zoorganize.Functions
{
    public class AnimalFunctions
    {
        private readonly AppDbContext inContext;
        private StaffFunctions? staffFunctions;

       
        public AnimalFunctions(AppDbContext inContext, StaffFunctions? staffFunctions = null)
        {
            this.inContext = inContext;
            this.staffFunctions = staffFunctions;
        }

        // Methode zum nachträglichen Setzen
        public void SetKeeperFunctions(StaffFunctions kf)
        {
            this.staffFunctions = kf;
        }
        //Funktionen, die sich mit Tieren beschäftigen, z.B. Berechnung des Alters, etc.
        //Funktionen, die ich für die Erstellung der Oberfläche hinsichtlich der Tiere brauche, z.B. Anzeige von Informationen, etc.
        //alle Tierarten 
        public async Task<List<Species>> GetSpecies() 
        { 
            return await inContext.Species.OrderBy(s => s.CommonName).ToListAsync();
        }

        public async Task<Species> GetSpeciesFromId(Guid id)
        {
            var species = await inContext.Species.FirstOrDefaultAsync(s => s.Id == id);
            if(species == null)
            {
                throw new KeyNotFoundException($"Animal with ID {id} not found");
            }
            return species;
        }
        //alle Tiere 
        public async Task<List<Animal>> GetAnimals()
        {
            return await inContext.Animals.Include(a => a.Species).OrderBy(s => s.Name).ToListAsync();
        }

        public async Task<Animal> GetAnimalById(Guid animalId)
        {
            var animal = await inContext.Animals.FirstOrDefaultAsync(s => s.Id == animalId);
            if (animal == null)
            {
                throw new KeyNotFoundException($"Animal with ID {animalId} not found");
            }
            return animal;
        }
        
        public async Task AddAnimal(AddAnimalType newAnimal)
        {
            
           var animal = new Animal
           {
                Id = Guid.NewGuid(),
                Name = newAnimal.Name,
                SpeciesId = newAnimal.SpeciesId,
                Age = newAnimal.Age,
                Note = newAnimal.Note,
                ArrivalDate = string.IsNullOrEmpty(newAnimal.ArrivalDate)
                    ? DateOnly.FromDateTime(DateTime.Now)
                    : DateOnly.Parse(newAnimal.ArrivalDate),
                Origin = (AnimalOrigin)newAnimal.Origin,
                BehavioralNotes = newAnimal.BehavioralNotes,
                Sex = newAnimal.Sex.HasValue ? (Sex)newAnimal.Sex.Value : Sex.neutral,
                IsNeutered = newAnimal.IsNeutered ?? false,
                IsPregnant = newAnimal.IsPregnant ?? false,
                HealthStatus = newAnimal.HealthStatus.HasValue
                    ? (HealthStatus)newAnimal.HealthStatus.Value
                    : HealthStatus.Healthy,
                WeightKg = newAnimal.WeightKg,
                InQuarantine = newAnimal.InQuarantine ?? false,
                Aggressive = newAnimal.Aggressive ?? false,
                RequiresSeparation = newAnimal.RequiresSeparation ?? false,
                ExternalZooStays = [],
                VeterinaryAppointments = [],
                CurrentEnclosureId = newAnimal.CurrentEnclosureId,
                KeeperId = newAnimal.KeeperId
           };

           animal.Species = await inContext.Species.FindAsync(animal.SpeciesId);

           //Lade CurrentEnclosure wenn gesetzt
            
            animal.CurrentEnclosure = await inContext.AnimalEnclosures.FindAsync(newAnimal.CurrentEnclosureId);
            animal.Keeper = await staffFunctions.GetStaffById(newAnimal.KeeperId);
            

            inContext.Animals.Add(animal);
            await inContext.SaveChangesAsync();
        }

        public async Task<Species> AddSpecies(AddSpeciesType newSpecies)
        {
            var species = new Species
            {
                Id = Guid.NewGuid(),
                // Basis
                CommonName = newSpecies.Name,
                ScientificName = newSpecies.ScientificName,

                // Haltung
                MinAreaPerAnimal = newSpecies.MinAreaPerAnimal ?? 0,
                MinGroupSize = newSpecies.MinGroupSize,
                MaxGroupSize = newSpecies.MaxGroupSize,
                IsSolitaryByNature = newSpecies.IsSolitaryByNature ?? false,

                // Klimaanforderungen
                MinTemperature = newSpecies.MinTemperature ?? 0,
                MaxTemperature = newSpecies.MaxTemperature ?? 30,
                MinHumidity = newSpecies.MinHumidity,
                MaxHumidity = newSpecies.MaxHumidity,
                RequiresOutdoorAccess = newSpecies.RequiresOutdoorAccess ?? false,

                // Sicherheitsmerkmale
                RequiredSecurityLevel = newSpecies.RequiredSecurityLevel.HasValue
                    ? (SecurityLevel)newSpecies.RequiredSecurityLevel.Value
                    : SecurityLevel.Low,
                IsDangerous = newSpecies.IsDangerous ?? false,
                RequiresSpecialPermit = newSpecies.RequiresSpecialPermit ?? false,

                // Infrastruktur
                RequiresWaterFeature = newSpecies.RequiresWaterFeature ?? false,
                RequiresClimbingStructures = newSpecies.RequiresClimbingStructures ?? false,
                RequiresShelter = newSpecies.RequiresShelter ?? false
            };

            inContext.Species.Add(species);
            await inContext.SaveChangesAsync();

            return species;
        }

        public async Task<Animal> AddAppointment(Guid animalId, AddAppointmentType newAppointment)
        {
            var animalExists = await inContext.Animals.AnyAsync(a => a.Id == animalId);
            if (!animalExists)
            {
                throw new KeyNotFoundException($"Animal with ID {animalId} not found");
            }

            // Erstelle Appointment
            var appointment = new VeterinaryAppointment
            {
                Id = Guid.NewGuid(),
                Title = newAppointment.Title,
                AppointmentDate = newAppointment.Date,
                Description = newAppointment.Notes,
                AnimalId = animalId,
                Animal = await inContext.Animals.FindAsync(animalId)
            };

            
            inContext.VeterinaryAppointments.Add(appointment);
            await inContext.SaveChangesAsync();

            var animal = await inContext.Animals
                .Include(a => a.VeterinaryAppointments)
                .Include(a => a.Species)
                .Include(a => a.CurrentEnclosure)
                .Include(a => a.Keeper)
                .FirstOrDefaultAsync(a => a.Id == animalId);

            return animal;
        }

        public async Task<Animal> AddExternalZooStay(Guid animalId, AddExternalZooStayType newStay)
        {
            // Validierung: Zoo-Name
            if (string.IsNullOrWhiteSpace(newStay.ZooName))
            {
                throw new ArgumentException("Zoo name cannot be empty");
            }

            // Validierung: Datumsbereich
            if (newStay.EndDate < newStay.StartDate)
            {
                throw new ArgumentException("End date cannot be before start date");
            }

            // Prüfe ob Tier existiert
            var animalExists = await inContext.Animals.AnyAsync(a => a.Id == animalId);
            if (!animalExists)
            {
                throw new KeyNotFoundException($"Animal with ID {animalId} not found");
            }

            // Erstelle ExternalZooStay
            var stay = new ExternalZooStay
            {
                Id = Guid.NewGuid(),
                ZooName = newStay.ZooName,
                StartDate = DateOnly.FromDateTime(newStay.StartDate),
                EndDate = newStay.EndDate.HasValue
                    ? DateOnly.FromDateTime(newStay.EndDate.Value)
                    : null,
                Description = newStay.Notes,
                AnimalId = animalId,
                Animal = await inContext.Animals.FindAsync(animalId)
            };

            // Füge direkt zum Context hinzu
            inContext.ExternalZooStays.Add(stay);
            await inContext.SaveChangesAsync();

            // Lade Tier mit allen Stays
            var animal = await inContext.Animals
                .Include(a => a.ExternalZooStays)
                .FirstOrDefaultAsync(a => a.Id == animalId);

            return animal ?? throw new KeyNotFoundException($"Animal with ID {animalId} not found after save");
        }

        public async Task<List<Animal>> DeleteAnimal(Guid animalId)
        {
            var animal = await inContext.Animals.FindAsync(animalId);

            if (animal == null)
            {
                throw new KeyNotFoundException($"Animal with ID {animalId} not found");
            }

            inContext.Animals.Remove(animal);
            await inContext.SaveChangesAsync();

            return await inContext.Animals.ToListAsync();
        }

        public async Task<List<Species>> DeleteSpecies(Guid speciesId)
        {
            var species = await inContext.Species.FindAsync(speciesId);

            if (species == null)
            {
                throw new KeyNotFoundException($"Species with ID {speciesId} not found");
            }

            inContext.Species.Remove(species);
            await inContext.SaveChangesAsync();

            return await inContext.Species.ToListAsync();
        }

        //Funktion, die Termine für Tiere holt z.B. Arzttermine
        public async Task<List<VeterinaryAppointment>> GetUpcomingAppointments(int daysAhead = 30)
        {
            var today = DateTime.Now.Date;
            var endDate = today.AddDays(daysAhead);

            return await inContext.VeterinaryAppointments
                .Include(a => a.Animal) 
                .Where(a => a.AppointmentDate >= today && a.AppointmentDate <= endDate)
                .OrderBy(a => a.AppointmentDate)
                .ToListAsync();
        }
        //Momentan nicht gebraucht, aber könnte später nützlich sein, um alle Termine anzuzeigen
        public async Task<List<VeterinaryAppointment>> GetAllAppointments()
        {
            return await inContext.VeterinaryAppointments
                .Include(a => a.Animal)
                .OrderBy(a => a.AppointmentDate)
                .ToListAsync();
        }

    }
}
