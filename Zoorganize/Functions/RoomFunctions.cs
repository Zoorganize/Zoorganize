using Microsoft.EntityFrameworkCore;
using Zoorganize.Database;
using Zoorganize.Database.Models;
using Zoorganize.Models.Api;

namespace Zoorganize.Functions
{
    public class RoomFunctions(AppDbContext context)
    {
        

        public async Task<List<AnimalEnclosure>> GetAnimalEnclosures()
        {
            return await context.AnimalEnclosures
                .Include(e => e.Animals)
                .Include(e => e.AllowedSpecies)
                .OrderBy(e => e.Name)
                .ToListAsync();
        }

        public async Task<List<StaffRooms>> GetStaffRooms()
        {
            return await context.StaffRooms
                .Include(s => s.AuthorizedStaff)
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<List<VisitorRoom>> GetVisitorRooms()
        {
            return await context.VisitorRooms
                .Include(v => v.Staff)
                .OrderBy(v => v.Name)
                .ToListAsync();
        }

        public async Task AddAnimalEnclosure(AddAnimalEnclosureType newEnclosure)
        {
            var enclosure = new AnimalEnclosure
            {
                Id = Guid.NewGuid(),
                Name = newEnclosure.Name,
                Location = newEnclosure.Location,
                Description = newEnclosure.Description,
                AreaInSquareMeters = newEnclosure.AreaInSquareMeters,
                Status = RoomStatus.Available,
                MaxCapacity = newEnclosure.MaxCapacity,
                IsOutdoor = newEnclosure.IsOutdoor,
                SecurityLevel = (SecurityLevel)newEnclosure.SecurityLevel,
                Type = (RoomType)newEnclosure.Type
            };

            context.AnimalEnclosures.Add(enclosure);
            await context.SaveChangesAsync();
        }

        public async Task AddStaffRoom(AddStaffRoomType newRoom)
        {
            var room = new StaffRooms
            {
                Name = newRoom.Name,
                Location = newRoom.Location,
                Description = newRoom.Description,
                AreaInSquareMeters = newRoom.AreaInSquareMeters,
                Status = RoomStatus.Available
            };

            context.StaffRooms.Add(room);
            await context.SaveChangesAsync();
        }

        public async Task AddVisitorRoom(AddVisitorRoomType newRoom)
        {
            var room = new VisitorRoom
            {
                Name = newRoom.Name,
                Location = newRoom.Location,
                Description = newRoom.Description,
                AreaInSquareMeters = newRoom.AreaInSquareMeters,
                Status = RoomStatus.Available,
                OpeningHours = newRoom.OpeningHours
            };

            context.VisitorRooms.Add(room);
            await context.SaveChangesAsync();
        }

        public async Task DeleteRoom(Guid roomId)
        {
            var room = await context.Rooms.FindAsync(roomId) ?? throw new KeyNotFoundException($"Room with ID {roomId} not found");
            context.Rooms.Remove(room);
            await context.SaveChangesAsync();
        }
    }

    
}
