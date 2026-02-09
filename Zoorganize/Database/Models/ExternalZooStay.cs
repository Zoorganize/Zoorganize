namespace Zoorganize.Database.Models
{
    public class ExternalZooStay
    {
        public Guid Id { get; set; }
        public Guid AnimalId { get; set; }
        public required Animal Animal { get; set; }
        public required string ZooName { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string? Description { get; set; }

    }
}
