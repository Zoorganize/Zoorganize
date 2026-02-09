namespace Zoorganize.Database.Models
{
    public class Animal
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public Guid SpeciesId { get; set; }
        public Species Species { get; set; }

        public string? Note { get; set; }
        public int? Age { get; set; }
        public DateOnly ArrivalDate { get; set; }

        public List<Staff> Keepers { get; set; } = [];

        //Herkunft
        public AnimalOrigin Origin { get; set; }

        public List<ExternalZooStay> ExternalZooStays { get; set; } = [];

        //Sex
        public Sex Sex { get; set; }
        public bool IsNeutered { get; set; }
        public bool IsPregnant { get; set; }

        //Gesundheit
        public HealthStatus HealthStatus { get; set; }
        public double? WeightKg { get; set; }
        public bool InQuarantine { get; set; }
        public List<VeterinaryAppointment> VeterinaryAppointments { get; set; } = [];

        //Verhalten
        public bool Aggressive { get; set; }
        public bool RequiresSeparation { get; set; }
        public string? BehavioralNotes { get; set; }

        //Gehege
        public Guid? CurrentEnclosureId { get; set; }
        public AnimalEnclosure? CurrentEnclosure { get; set; }
        public DateOnly? EnclosureAssignedSince { get; set; }
    }

    public enum Sex
    {
        female,
        male,
        neutral
    }
    public enum HealthStatus
    {
        Healthy,
        Sick,
        Recovering,
        UnderObservation
    }

    public enum AnimalOrigin
    {
        BornInZoo,
        TransferredFromAnotherZoo,
        Rescued,
        Other
    }
}
