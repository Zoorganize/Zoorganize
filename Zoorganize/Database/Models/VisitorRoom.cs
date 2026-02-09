namespace Zoorganize.Database.Models
{
    public class VisitorRoom : Room
    {
        public required string OpeningHours { get; set; }
            public List<Staff> Staff { get; set; } = [];
    }
}
