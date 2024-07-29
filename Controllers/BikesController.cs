using BikeTogether.Models;
using BikeTogether.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BikeTogether.Controllers
{
    public class BikesController : Controller
    {
        private readonly ProjectDbContext _dbContext;
        public BikesController(ProjectDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult BikesDetail(Guid id)
        {
            var bikes = _dbContext.Bikes.FirstOrDefault(h => h.Id == id);
            if (bikes == null)
            {
                return NotFound();
            }
            string bikesIdString = id.ToString();
            var images = _dbContext.ImageCategories
                             .Where(ic => ic.BikesId.ToString() == bikesIdString)
                             .ToList();
            var userIdString = HttpContext.Session.GetString("UserId");
            Guid userId;
            bool hasBookings = false;
            if (Guid.TryParse(userIdString, out userId))
            {
                var listBookings = _dbContext.BookingBikes.Where(ui => ui.CustomerId == userId && ui.BikeId == bikes.Id);
                var countListBookings = listBookings.Count();
                Console.WriteLine("Guid BikesId Booking: " + bikes.Id);
                Console.WriteLine("Guid CustomerId" + userIdString);
                Console.WriteLine("Count " + countListBookings);
                if (countListBookings > 0)
                {
                    hasBookings = true;

                }
                else
                {
                    hasBookings = false;
                }
            }
            var viewModel = new BikesDetailViewModel
            {
                Bikes = bikes,
                Images = images,
                BookingBike = new BookingBike(),
                IsScheduled = hasBookings
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AddNoteBookingBikes(Guid BikesId, string note)
        {
            Console.WriteLine("Guid HouseId: " + BikesId);

            if (HttpContext.Session.GetString("Username") != null)
            {
                var email = HttpContext.Session.GetString("Username");
                var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);

                if (user != null)
                {
                    var customerId = user.Id;

                    var bookingBikes = new BookingBike
                    {
                        BikeId = BikesId,
                        Note = note,
                        CreatedAt = DateTime.Now,
                        CustomerId = customerId
                    };

                    _dbContext.BookingBikes.Add(bookingBikes);
                    _dbContext.SaveChanges();
                    TempData["SuccessMessage"] = "Thuê xe thành công!";
                    return RedirectToAction("BikesDetail", "Bikes", new { id = BikesId });
                }
                else
                {

                    return RedirectToAction("BikesDetail", "Bikes", new { id = BikesId });
                }
            }
            return RedirectToAction("BikesDetail", "Bikes", new { id = BikesId });
        }
    }
}
