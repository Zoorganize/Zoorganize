using System;
using System.Collections.Generic;
using System.Text;

namespace Zoorganize.Models.Api
{
    public class AddStaffRoomType
    {
        public required string Name { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }
        public double? AreaInSquareMeters { get; set; }
    }
}
