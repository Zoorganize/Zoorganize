using Microsoft.EntityFrameworkCore;
using Zoorganize.Database;
using Zoorganize.Database.Models;
using Zoorganize.Models.Api;

namespace Zoorganize.Functions
{
    public class KeeperFunctions(AnimalFunctions animalFunctions, AppDbContext inContext)
    {
        //Funktionen, die sich mit Pflegern beschäftigen, z.B. Berechnung der Arbeitszeit, etc.
        //Funktionen, die ich für die Erstellung der Oberfläche hinsichtlich der Pfleger brauche, z.B. Anzeige von Informationen, etc.
        //neuen Pfleger anlegen => Name, gebiurtstag, gehalt, notizen
        //alle Pfleger holen (bestimmte Sortierung?)
        //Pfleger löschen
        public async Task<List<Staff>> GetStaff()
        {
            return await inContext.Staff.ToListAsync();
        }

        public async Task<List<Staff>> GetStaffByIds(List<Guid> staffList)
        {
            return await inContext.Staff
                .Where(s => staffList.Contains(s.Id))
        .       ToListAsync();
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
