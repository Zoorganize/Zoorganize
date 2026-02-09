namespace Zoorganize.Database.Models
{
    public class Staff
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Sex Sex { get; set; }
        
        public JobRole JobRole { get; set; }
        public EmploymentType EmploymentType { get; set; }
        public float YearlySalary { get; set; }
        public string? ContactInfo { get; set; }
        public string? Address { get; set; }
        public DateOnly HireDate { get; set; }
        public DateOnly? ExitDate { get; set; }
        public string? Notes { get; set; }
        public bool IsActive { get; set; }

        public List<Species> AuthorizedSpecies { get; set; } = [];
        public List<Animal> AssignedAnimals { get; set; } = [];
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

    public enum JobRole
    {
        Keeper,
        Veterinarian,
        Guide,
        Maintenance,
        Administration,
        Sales,
        Security,
        Other
    }

}
