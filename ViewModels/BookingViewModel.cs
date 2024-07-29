namespace BikeTogether.ViewModels
{
    public class BookingViewModel
    {
        public Guid bookingId { get; set; }
        public string bikeName { get; set; }
        public string bikeAddress { get; set; }
        public string bikePrice { get; set; }
        public DateTime bikeCreatedAt { get; set; }
        public string bikeTittle { get; set; }
    }
}
