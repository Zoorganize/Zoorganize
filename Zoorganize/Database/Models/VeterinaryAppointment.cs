namespace Zoorganize.Database.Models
{
    public class VeterinaryAppointment
    {
        public Guid Id { get; set; }
        public Guid AnimalId { get; set; }
        public required Animal Animal { get; set; }

        public required string Title { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string? Description { get; set; }

    }
}
