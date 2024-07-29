using BikeTogether.Models;

namespace BikeTogether.ViewModels
{
    public class BikesViewModel
    {
        //public List<Bike> Bikes { get; set; }
        public List<BikeWithFullAddressViewModel> Bikes { get; set; }
        public List<ImageCategory> Images { get; set; }
    }
}
