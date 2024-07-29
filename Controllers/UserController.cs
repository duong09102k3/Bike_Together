using BikeTogether.Models;
using BikeTogether.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BikeTogether.Controllers
{
    public class UserController : Controller
    {
        private readonly ProjectDbContext _context;
        public UserController(ProjectDbContext context)
        {
            _context = context;
        }

        public IActionResult UserManage()
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

        public IActionResult UserInfo()
        {
            if (HttpContext.Session.GetString("Username") != null)
            {
                var email = HttpContext.Session.GetString("Username");
                User user = new User();
                user = _context.Users.FirstOrDefault(u => u.Email == email);
                if (user != null)
                {
                    return View(user);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult UserInfoBikesBooking()
        {
            if (HttpContext.Session.GetString("Username") != null)
            {
                User user = new User();
                List<Bike> bikes = new List<Bike>();
                List<BookingBike> bookingBikes = new List<BookingBike>();
                List<BookingViewModel> bookingViewModels = new List<BookingViewModel>();
                var email = HttpContext.Session.GetString("Username");
                user = _context.Users.FirstOrDefault(u => u.Email == email);
                if (user != null)
                {
                    bookingBikes = _context.BookingBikes.Where(b => b.CustomerId == user.Id).ToList();
                    foreach (var bookingBike in bookingBikes)
                    {
                        Bike bike = _context.Bikes.FirstOrDefault(h => h.Id == bookingBike.BikeId);
                        if (bike != null)
                        {
                            BookingViewModel bookingViewModel = new BookingViewModel();
                            bookingViewModel.bookingId = bookingBike.Id;
                            bookingViewModel.bikePrice = bike.Price;
                            bookingViewModel.bikeName = bike.NameBike;
                            bookingViewModel.bikeTittle = bookingBike.Note;
                            bookingViewModel.bikeCreatedAt = (DateTime)bookingBike.CreatedAt;
                            bookingViewModel.bikeAddress = bike.Address;
                            bookingViewModels.Add(bookingViewModel);
                        }
                    }
                    UserInfoBikesBookingViewModel userInfoBikesBookingViewModel = new UserInfoBikesBookingViewModel
                    {
                        user = user,
                        bookingList = bookingViewModels
                    };
                    return View(userInfoBikesBookingViewModel);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult DeleteBooking(Guid bookingId)
        {
            if (HttpContext.Session.GetString("Username") != null)
            {
                var email = HttpContext.Session.GetString("Username");
                var user = _context.Users.FirstOrDefault(u => u.Email == email);
                if (user != null)
                {
                    var booking = _context.BookingBikes.FirstOrDefault(b => b.Id == bookingId && b.CustomerId == user.Id);
                    if (booking != null)
                    {
                        _context.BookingBikes.Remove(booking);
                        _context.SaveChanges();
                        return RedirectToAction("UserInfoBikesBooking");
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }

        //DataTable=====================================================================================
        [HttpGet]
        public string GetListUser()
        {
            List<User> users = _context.Users.ToList();
            foreach (var user in users)
            {
                var roleName = _context.Roles.FirstOrDefault(r => r.Id == user.RoleId)?.Name;
                user.RoleId = roleName ?? "Không xác định";
            }
            var value = JsonConvert.SerializeObject(new { data = users });

            return value;
        }

        // Cập nhật========================================================================================
        [HttpPost]
        public async Task<int> CapNhat([FromBody] User userObject)
        {
            var result = -1;

            try
            {
                _context.Users.Update(userObject);
                await _context.SaveChangesAsync();
                result = 1;
            }
            catch (DbUpdateConcurrencyException)
            {
                result = 0;
            }
            return result;
        }

        //Chi tiết=====================================================================================
        [HttpGet]
        public JsonResult ChiTiet(Guid userId)
        {
            var user = (from u in _context.Users
                        join r in _context.Roles on u.RoleId equals r.Id
                        where u.Id == userId
                        select new
                        {
                            name = u.Name,
                            phoneNumber = u.PhoneNumber,
                            email = u.Email,
                            password = u.Password,                          
                            roleId = r.Id

                        }).ToList();
            return Json(user);
        }

        // Xóa=====================================================================================================
        [HttpPost]

        public async Task<int> DeleteConfirmed(Guid userId)
        {
            var result = -1;
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                result = 1;
            }
            else
            {
                result = 0;
            }

            return result;
        }


        //DataTable=====================================================================================
        [HttpPost]
        public JsonResult getDataTablesWithUserId(Guid userId)
        {
            var listBooking = _context.BookingBikes.Where(bc => bc.CustomerId == userId).ToList();

            List<object> objectArray = new List<object>();
            foreach (var booking in listBooking)
            {
                if (!string.IsNullOrEmpty(booking.BikeId.ToString()))
                {
                    var bikeQuery = (from h in _context.Bikes
                                      join l in _context.Locations on h.OfLocationId equals l.Id
                                      where h.Id == booking.BikeId
                                      select new
                                      {
                                          id = h.Id,
                                          idBooking = booking.Id,
                                          nameBike = h.NameBike,
                                          bikeStatus = h.BikeStatus,
                                          price = h.Price,
                                          address = h.Address,
                                          locationName = l.Name
                                      }).FirstOrDefault();

                    if (bikeQuery != null)
                    {
                        objectArray.Add(bikeQuery);
                    }
                }
            }
            return Json(new { data = objectArray });
        }
    }
}
