namespace Zoorganize.Database.Models
{
    public class Zoo
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        private List<Staff> Keepers { get; set; } = [];
        private List<Animal> Animals { get; set; } = [];
        
    }
}
