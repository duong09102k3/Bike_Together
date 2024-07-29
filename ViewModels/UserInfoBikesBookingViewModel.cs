using BikeTogether.Models;

namespace BikeTogether.ViewModels
{
    public class UserInfoBikesBookingViewModel
    {
        public User user { get; set; }
        public List<BookingViewModel> bookingList { get; set; }
    }
}
