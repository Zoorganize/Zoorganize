namespace Zoorganize.Database.Models
{
    public class AnimalEnclosure : Room
    {
        public bool IsOutdoor { get; set; }
        public List<Animal> Animals { get; set; } = [];
        public int MaxCapacity { get; set; }
        public bool TemperatureControlled { get; set; }
        public bool MixedSpeciesAllowed { get; set; }

        //Klimaanforderungen
        public double MinTemperature { get; set; }
        public double MaxTemperature { get; set; }
        public double? MinHumidity { get; set; }
        public double? MaxHumidity { get; set; }

        //Sicherheitsmerkmale
        public bool IsEscapeProof { get; set; }
        public SecurityLevel SecurityLevel { get; set; }


        //Infrastruktur
        public bool HasWaterFeature { get; set; }
        public bool HasClimbingStructures { get; set; }
        public bool HasShelter { get; set; }

         
        public List<Species> AllowedSpecies { get; set; } = [];
    }

    public enum SecurityLevel
    {
        Low,
        Medium,
        High,
        VeryHigh
    }
    
}
