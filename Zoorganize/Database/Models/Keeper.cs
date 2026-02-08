using System.ComponentModel.DataAnnotations.Schema;

namespace Zoorganize.Database.Models
{
    public class Keeper
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Sex Sex { get; set; }
        public beschäftigunmgg

            public EmploymentType EmploymentType { get; set; }
        public float Paid
        public string? ContactInfo { get; set; }
        public string? Address { get; set; }
        public DateOnly HireDate { get; set; }
        public DateOnly? ExitDate { get; set; }
        public string? Notes { get; set; }
        public bool IsActive { get; set; }

        public List<Species> ResponsibleSpecies { get; set; } = [];

    }
    //Beschäftigungsart
    public enum  EmploymentType
    {
        FullTime,
        PartTime,
        Intern,
        Volunteer,
        Contractor
    }
    {
        
    }
}
