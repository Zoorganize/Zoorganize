using System;
using System.Collections.Generic;
using System.Text;

namespace Zoorganize.Database.Models
{
    public class VeterinaryAppointment
    {
        public Guid Id { get; set; }
        public Guid AnimalId { get; set; }
        public Animal Animal { get; set; }

        public string Title { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public string? Description { get; set; }

    }
}
