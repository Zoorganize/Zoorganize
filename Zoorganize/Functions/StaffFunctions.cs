using Microsoft.EntityFrameworkCore;
using Zoorganize.Database;
using Zoorganize.Database.Models;
using Zoorganize.Models.Api;

namespace Zoorganize.Functions
{
    public class StaffFunctions(AnimalFunctions animalFunctions, AppDbContext inContext)
    {
        
        public async Task<List<Staff>> GetStaff()
        {
            return await inContext.Staff.Include(s => s.AuthorizedSpecies).ToListAsync();
        }

        public async Task<Staff> GetStaffById(Guid staffId)
        {
            var keeper =  await inContext.Staff.FirstOrDefaultAsync(s => s.Id == staffId);
            if (keeper == null)
            {
                throw new KeyNotFoundException($"Staff with Id {staffId} not found");
            }
            return keeper;

        }

        public async Task<List<Staff>> GetKeepers()
        {
            var keepers = await inContext.Staff
                .Where(s => s.JobRole == JobRole.Keeper)
                .Include(s => s.AuthorizedSpecies)
                .OrderBy(s => s.Name) 
                .ToListAsync();

            if (keepers == null)
            {
                throw new KeyNotFoundException($"Staff with Keeper Job not found");
            }
            return keepers;
        }
        public async Task<List<Staff>> AddPersonal(AddStaffType newStaff)
        {
            var staff = new Staff
            {
                Id = Guid.NewGuid(),
                Name = newStaff.Name,
                Sex = (Sex)newStaff.Sex,
                JobRole = (JobRole)newStaff.JobRole,
                EmploymentType = (EmploymentType)newStaff.EmploymentType,
                YearlySalary = newStaff.YearlySalary ?? 0f,
                ContactInfo = newStaff.ContactInfo,
                Address = newStaff.Address,
                HireDate = DateOnly.TryParse(newStaff.HireDate, out var hire)
                    ? hire
                    : DateOnly.FromDateTime(DateTime.Now),
                ExitDate = DateOnly.TryParse(newStaff.ExitDate, out var exit)
                    ? exit
                    : null,
                Notes = newStaff.Notes,
                IsActive = newStaff.IsActive ?? true
            };
            
            foreach(var spec in newStaff.AuthorizedSpecies)
            {
                staff.AuthorizedSpecies.Add(await animalFunctions.GetSpeciesFromId(spec));
            }

            inContext.Staff.Add(staff);
            await inContext.SaveChangesAsync();
            return await GetStaff();
        }

        public async Task<List<Staff>> DeletePersonal(Guid staffId)
        {
            var staff = await inContext.Staff.FindAsync(staffId);
            if (staff == null)
            {
                throw new KeyNotFoundException($"Staff with ID {staffId} not found");
            }

            inContext.Staff.Remove(staff);
            await inContext.SaveChangesAsync();

            return await inContext.Staff.ToListAsync();
        }
    }
}
