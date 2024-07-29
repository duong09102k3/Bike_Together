using BikeTogether.Models;
using BikeTogether.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BikeTogether.Controllers
{
    public class BookingBikeController : Controller
    {
        private readonly ProjectDbContext _context;

        public BookingBikeController(ProjectDbContext context)
        {
            _context = context;
        }

        public IActionResult BookingBikeView()
        {
            if (HttpContext.Session.GetString("Role") == "role1")
            {
                return View();
            }
            else
            {
                return RedirectToAction("UserInfo", "User");
            }
        }


        /*
        public ActionResult ModifyNoteBookingCalender(Guid bookingId, string note)
        {
            var bookingCalender = _context.BookingCalenders.FirstOrDefault(b => b.Id == bookingId);
            if (bookingCalender != null)
            {
                bookingCalender.Note = note;
                _context.SaveChanges();
                return RedirectToAction("UserInfoHouseBooking", "User");
            }
            else
            {
                return RedirectToAction("UserInfoHouseBooking", "User");
            }
        }
        */
        [HttpPost]
        public async Task<int> ModifyNoteBookingBike([FromBody] BookingUpdateModel model)
        {
            var result = -1;
            try
            {
                var booking = await _context.BookingBikes.FindAsync(model.BookingId);
                if (booking != null)
                {
                    booking.Note = model.Note;
                    await _context.SaveChangesAsync();
                }

                result = 1;
            }
            catch (DbUpdateConcurrencyException)
            {
                result = 0;
            }
            return result;
        }
        //DataTable=====================================================================================
        [HttpGet]
        public JsonResult GetBookings()
        {
            var bookingDetails = (from bc in _context.BookingBikes
                                  join u in _context.Users on bc.CustomerId equals u.Id
                                  join h in _context.Bikes on bc.BikeId equals h.Id
                                  select new
                                  {
                                      bookingId = bc.Id,
                                      bikeId = h.Id,
                                      bikePrice = h.Price,
                                      bikeStatus = h.BikeStatus,
                                      customerId = u.Id,
                                      customerName = u.Name,
                                      customerPhone = u.PhoneNumber,
                                  }).ToList();
            return Json(new { data = bookingDetails });
        }



        //Chi tiết=====================================================================================
        [HttpGet]
        public JsonResult ChiTiet(Guid bookingBikesId)
        {
            var bookingDetails = (from bc in _context.BookingBikes
                                  join u in _context.Users on bc.CustomerId equals u.Id
                                  join h in _context.Bikes on bc.BikeId equals h.Id
                                  join l in _context.Locations on h.OfLocationId equals l.Id
                                  where bc.Id == bookingBikesId
                                  select new
                                  {
                                      bookingId = bc.Id,
                                      bookingNote = bc.Note,

                                      customerName = u.Name,
                                      customerPhone = u.PhoneNumber,
                                      customerEmail = u.Email,

                                      bikeTitle = h.NameBike,
                                      ownerName = h.OwnerName,
                                      ownerPhone = h.OwnerPhone,
                                      address = h.Address,
                                      price = h.Price,
                                      description = h.Description,

                                      locationName = l.Name

                                  }).ToList();

            return Json(bookingDetails);
        }

        // Cập nhật HouseStatus=====================================================================================================
        [HttpPost]
        public async Task<int> UpdateBikeStatus([FromBody] Bike bikeObject)
        {
            var result = -1;
            var bike = await _context.Bikes.FindAsync(bikeObject.Id);
            if (bike != null)
            {

                bike.BikeStatus = bikeObject.BikeStatus;
                _context.Update(bike);
                await _context.SaveChangesAsync();
                result = 1;
            }
            else
            {
                result = 0;
            }

            return result;
        }

        // Xóa=====================================================================================================
        [HttpPost]
        public async Task<int> DeleteConfirmed(Guid bookingBikesId)
        {
            var result = -1;
            var bc = await _context.BookingBikes.FindAsync(bookingBikesId);
            if (bc != null)
            {
                _context.BookingBikes.Remove(bc);
                await _context.SaveChangesAsync();
                result = 1;
            }
            else
            {
                result = 0;
            }

            return result;
        }
    }
}
