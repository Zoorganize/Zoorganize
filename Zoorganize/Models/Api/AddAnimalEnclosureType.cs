using System;
using System.Collections.Generic;
using System.Text;

namespace Zoorganize.Models.Api
{
    public class AddAnimalEnclosureType
    {
        public required string Name { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }
        public double AreaInSquareMeters { get; set; }
        public int MaxCapacity { get; set; }
        public bool IsOutdoor { get; set; }
        public int SecurityLevel { get; set; }
        public int Type { get; set; }
    }
}
