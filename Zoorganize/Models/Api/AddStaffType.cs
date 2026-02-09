namespace Zoorganize.Models.Api
{
    public class AddStaffType
    {
        public required string Name { get; set; }
        public int Sex { get; set; }

        public int JobRole { get; set; }
        public int EmploymentType { get; set; }
        public float? YearlySalary { get; set; }
        public string? ContactInfo { get; set; }
        public string? Address { get; set; }
        public string? HireDate { get; set; }  // Format: "yyyy-MM-dd"
        public string? ExitDate { get; set; }  // Format: "yyyy-MM-dd"
        public string? Notes { get; set; }
        public bool? IsActive { get; set; }

        public List<Guid> AuthorizedSpecies { get; set; } = [];
        
    }
}
