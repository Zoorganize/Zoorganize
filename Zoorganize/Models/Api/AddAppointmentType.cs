using System;
using System.Collections.Generic;
using System.Text;

namespace Zoorganize.Models.Api
{
    public class AddAppointmentType
    {
        public required string Title { get; set; }
        public required DateTime Date { get; set; }  
        public string? Notes { get; set; }
    }
}
