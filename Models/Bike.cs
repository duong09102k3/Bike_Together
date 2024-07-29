using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BikeTogether.Models
{
    public class Bike
    {
        [Key]
        public Guid Id { get; set; }

        [Column(TypeName = "nvarchar(1000)")]
        public string NameBike { get; set; }

        [Column(TypeName = "nvarchar(1000)")]
        public string Address { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string Price { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string OwnerName { get; set; }

        [Column(TypeName = "varchar(15)")]
        public string OwnerPhone { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string BikeStatus { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Description { get; set; }


        [Column(TypeName = "varchar(20)")]
        public string OfLocationId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }


    }
}
