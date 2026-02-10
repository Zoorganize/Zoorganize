namespace Zoorganize.Models.Api
{
    public class AddVisitorRoomType
    {
        public required string Name { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }
        public double? AreaInSquareMeters { get; set; }
        public required string OpeningHours { get; set; }

        public int Type { get; set; }
    }
}
