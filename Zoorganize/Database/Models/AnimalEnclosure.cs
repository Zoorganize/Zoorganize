using System;
using System.Collections.Generic;
using System.Text;

namespace Zoorganize.Database.Models
{
    public class AnimalEnclosure
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }
        public GehegeStatus Status { get; set; }

        public double AreaInSquareMeters { get; set; }
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

        //TODO: Implement AllowedSpecies 
        //public List<Species> AllowedSpecies { get; set; } = [];
    }

    public enum SecurityLevel
    {
        Low,
        Medium,
        High,
        VeryHigh
    }
    public enum GehegeStatus
    {
        Available,
        UnderMaintenance,
        Closed,
        Renovation
    }
}
