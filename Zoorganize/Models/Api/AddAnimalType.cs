namespace Zoorganize.Models.Api
{
    public class AddAnimalType
    {
        public required string Name { get; set; }
        public Guid SpeciesId { get; set; }

        public string? Note { get; set; }
        public int? Age { get; set; }
        public string? ArrivalDate { get; set; }

        public List<Guid> Keepers { get; set; } = [];

        //Herkunft
        public int Origin { get; set; }


        //Sex
        public int? Sex { get; set; }
        public bool? IsNeutered { get; set; }
        public bool? IsPregnant { get; set; }

        //Gesundheit
        public int? HealthStatus { get; set; }
        public double? WeightKg { get; set; }
        public bool? InQuarantine { get; set; }
        

        //Verhalten
        public bool? Aggressive { get; set; }
        public bool? RequiresSeparation { get; set; }
        public string? BehavioralNotes { get; set; }

        
    }

}
