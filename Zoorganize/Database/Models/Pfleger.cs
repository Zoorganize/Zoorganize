using System.ComponentModel.DataAnnotations.Schema;

namespace Zoorganize.Database.Models
{
    [Table("Pfleger")]
    public class Pfleger
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
