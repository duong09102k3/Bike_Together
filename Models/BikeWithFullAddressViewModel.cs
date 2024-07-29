namespace BikeTogether.Models
{
    public class BikeWithFullAddressViewModel
    {
        public Guid Id { get; set; }
        public string NameBike { get; set; }
        public string Address { get; set; }
        public string FullAddress { get; set; }
        public string Price { get; set; }
        public string OwnerName { get; set; }
        public string OwnerPhone { get; set; }
        public string BikeStatus { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
