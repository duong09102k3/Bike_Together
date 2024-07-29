using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BikeTogether.Models
{
    public class Location
    {
        [Key]
        [Column(TypeName = "varchar(20)")]
        public string Id { get; set; }
        [Column(TypeName = "nvarchar(1000)")]
        public string Name { get; set; }
        public string? ParentId { get; set; }
    }
}
