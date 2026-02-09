using System;
using System.Collections.Generic;
using System.Text;

namespace Zoorganize.Models.Api
{
    public class AddExternalZooStayType
    {
        public required string ZooName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Notes { get; set; }
    }
}
