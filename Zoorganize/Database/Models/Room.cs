namespace Zoorganize.Database.Models
{
    public class Room
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }
        public double? AreaInSquareMeters { get; set; }
        public RoomStatus Status { get; set; }

        public RoomType Type { get; set; }
    }

    public enum RoomStatus
    {
        Available,
        UnderMaintenance,
        Closed,
        Renovation
    }

    public enum RoomType
    {
        AnimalEnclosure,
        StaffRoom,
        VisitorRoom
    }
}
