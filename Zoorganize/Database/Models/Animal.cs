using System;
using System.Collections.Generic;
using System.Text;

namespace Zoorganize.Database.Models
{
    public class Animal
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Species { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateOnly ArrivalDate { get; set; }
        public List<VeterinaryAppointment> VeterinaryAppointments { get; set; } = [];

        public List<ExternalZooStay> ExternalZooStays { get; set; } = [];
    }
}
