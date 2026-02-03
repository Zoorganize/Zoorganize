using System.ComponentModel.DataAnnotations.Schema;

namespace Zoorganize.Database.Models
{
    public class Keeper
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? ContactInfo { get; set; }
        public string? Address { get; set; }
        public DateOnly HireDate { get; set; }
        public DateOnly? ExitDate { get; set; }
        public string? Notes { get; set; }
        public bool IsActive { get; set; }

        public List<Species> ResponsibleSpecies { get; set; } = [];

    }
}
