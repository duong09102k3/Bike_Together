using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BikeTogether.Models
{
    public class Role
    {
        [Key]
        [Column(TypeName = "varchar(20)")]
        public string Id { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
