using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BikeTogether.Models
{
    public class ImageCategory
    {
        [Key]
        public Guid Id { get; set; }
        [Column(TypeName = "nvarchar(MAX)")]
        public string? Url { get; set; }
        [Column(TypeName = "varchar(50)")]
        public Guid BikesId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
