namespace Zoorganize.Database.Models
{
    public class StaffRooms : Room
    {
        public List<Staff> AuthorizedStaff { get; set; } = [];
    }
}
