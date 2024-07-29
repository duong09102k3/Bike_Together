using BikeTogether.Models;
public class BikesDetailViewModel
    {
        public Bike Bikes { get; set; }
        public List<ImageCategory> Images { get; set; }
        public BookingBike BookingBike{ get; set; }
        public bool IsScheduled { get; set; }
    }

