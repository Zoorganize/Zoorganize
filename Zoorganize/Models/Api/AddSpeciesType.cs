using Zoorganize.Database.Models;

namespace Zoorganize.Models.Api
{
    public class AddSpeciesType
    {
        public required string Name { get; set; }
        public string? ScientificName { get; set; }

        //Haltung
        public double? MinAreaPerAnimal { get; set; }
        public int? MinGroupSize { get; set; }
        public int? MaxGroupSize { get; set; }
        public bool? IsSolitaryByNature { get; set; }

        //Klimaanforderungen
        public double? MinTemperature { get; set; }
        public double? MaxTemperature { get; set; }
        public double? MinHumidity { get; set; }
        public double? MaxHumidity { get; set; }
        public bool? RequiresOutdoorAccess { get; set; }

        //Sicherheitsmerkmale
        public int? RequiredSecurityLevel { get; set; }
        public bool? IsDangerous { get; set; }
        public bool? RequiresSpecialPermit { get; set; }

        //Infrastruktur
        public bool? RequiresWaterFeature { get; set; }
        public bool? RequiresClimbingStructures { get; set; }
        public bool? RequiresShelter { get; set; }
    }
}
