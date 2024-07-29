using BikeTogether.Models;
using BikeTogether.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BikeTogether.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProjectDbContext _context;

        public HomeController(ProjectDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        //public JsonResult GetBikesData()
        //{
        //    var bikes = _context.Bikes.Where(h => h.BikeStatus == "Available").ToList();
        //    var bikesIds = bikes.Select(h => h.Id).ToList();
        //    var images = _context.ImageCategories.Where(ic => bikesIds.Contains(ic.BikesId)).ToList();
        //    var viewBikesModel = new BikesViewModel
        //    {
        //        Bikes = bikes,
        //        Images = images
        //    };

        //    return Json(viewBikesModel);
        //}
        //public JsonResult Search(string keyword)
        //{
        //    // Phân tách chuỗi thành các giá trị riêng lẻ
        //    var keywordArray = keyword.Split(',');

        //    // Khởi tạo các biến để lưu trữ các giá trị từ chuỗi
        //    var priceString = "";
        //    var address = "";


        //    // Lặp qua từng phần tử trong mảng chuỗi
        //    foreach (var item in keywordArray)
        //    {
        //        // Kiểm tra nếu phần tử chứa từ "k"
        //        if (item.Contains("k"))
        //        {
        //            // Lấy giá trị giá từ chuỗi
        //            priceString = item.Replace(" k", "").Trim();
        //        }

        //        // Nếu không phải là giá trị giá hoặc diện tích, giả sử là địa chỉ
        //        else
        //        {
        //            // Lấy giá trị địa chỉ từ chuỗi
        //            address += item.Trim() + " ";
        //        }
        //    }

        //    // Tìm kiếm trong cơ sở dữ liệu
        //    var bikesSearch = _context.Bikes.Where(p =>
        //        (string.IsNullOrEmpty(priceString) || p.Price.Contains(priceString)) &&
        //        (string.IsNullOrEmpty(address) || p.Address.Contains(address.Trim()))
        //    ).ToList();
        //    var images = _context.ImageCategories.ToList();
        //    var viewBikesModel = new BikesViewModel
        //    {
        //        Bikes = bikesSearch,
        //        Images = images
        //    };
        //    // Trả về kết quả
        //    if (bikesSearch.Count == 0)
        //    {
        //        return Json(new { error = "Không tìm thấy dữ liệu!" });
        //    }
        //    else
        //    {
        //        return Json(viewBikesModel);
        //    }
        //}
        //private string GetFullAddress(string locationId)
        //{
        //    var location = _context.Locations.FirstOrDefault(l => l.Id == locationId);
        //    if (location == null) return string.Empty;

        //    var addressComponents = new List<string> { location.Name };
        //    while (location.ParentId != "#" && !string.IsNullOrEmpty(location.ParentId))
        //    {
        //        location = _context.Locations.FirstOrDefault(l => l.Id == location.ParentId);
        //        if (location != null)
        //        {
        //            addressComponents.Insert(0, location.Name); // Add at the beginning
        //        }
        //    }

        //    return string.Join(", ", addressComponents);
        //}




        //public JsonResult GetLocations()
        //{
        //    var locations = _context.Locations.ToList();
        //    var bikes = _context.Bikes.ToList();

        //    var viewModel = new ViewAddress
        //    {
        //        Locations = locations,
        //        Bikes = bikes
        //    };

        //    return Json(viewModel);
        //}

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}

        private string GetFullAddress(string locationId)
        {
            var location = _context.Locations.FirstOrDefault(l => l.Id == locationId);
            if (location == null) return string.Empty;

            var addressComponents = new List<string> { location.Name };
            while (location.ParentId != "#" && !string.IsNullOrEmpty(location.ParentId))
            {
                location = _context.Locations.FirstOrDefault(l => l.Id == location.ParentId);
                if (location != null)
                {
                    addressComponents.Insert(0, location.Name); // Thêm vào cuối danh sách để đảm bảo thứ tự từ nhỏ nhất đến lớn nhất
                }
            }

            addressComponents.Reverse(); // Đảo ngược danh sách để có thứ tự đúng
            return string.Join(", ",addressComponents);
        }



        public JsonResult GetBikesData()
        {
            var bikes = _context.Bikes.Where(h => h.BikeStatus == "Available").ToList();
            var bikesIds = bikes.Select(h => h.Id).ToList();
            var images = _context.ImageCategories.Where(ic => bikesIds.Contains(ic.BikesId)).ToList();

            var bikesWithFullAddress = bikes.Select(b => new BikeWithFullAddressViewModel
            {
                Id = b.Id,
                NameBike = b.NameBike,
                Address = b.Address,
                FullAddress = GetFullAddress(b.OfLocationId),
                Price = b.Price,
                OwnerName = b.OwnerName,
                OwnerPhone = b.OwnerPhone,
                BikeStatus = b.BikeStatus,
                Description = b.Description,
                CreatedAt = b.CreatedAt,
                ModifiedAt = b.ModifiedAt
            }).ToList();

            var viewBikesModel = new BikesViewModel
            {
                Bikes = bikesWithFullAddress,
                Images = images
            };

            return Json(viewBikesModel);
        }


        public JsonResult Search(string keyword)
        {
            var keywordArray = keyword.Split(',');

            var priceString = "";
            var address = "";

            foreach (var item in keywordArray)
            {
                if (item.Contains("k"))
                {
                    priceString = item.Replace(" k", "").Trim();
                }
                else
                {
                    address += item.Trim() + " ";
                }
            }

            address = address.Trim(); // Loại bỏ khoảng trắng thừa ở cuối chuỗi address

            // Lấy tất cả các bikes trước, sau đó lọc dựa trên FullAddress
            var bikes = _context.Bikes.ToList();
            var bikesWithFullAddress = bikes.Select(b => new
            {
                Bike = b,
                FullAddress = GetFullAddress(b.OfLocationId)
            });

            var bikesSearch = bikesWithFullAddress.Where(b =>
                (string.IsNullOrEmpty(priceString) || b.Bike.Price.Contains(priceString)) &&
                (string.IsNullOrEmpty(address) || b.FullAddress.Contains(address))
            ).Select(b => new BikeWithFullAddressViewModel
            {
                Id = b.Bike.Id,
                NameBike = b.Bike.NameBike,
                Address = b.Bike.Address,
                FullAddress = b.FullAddress,
                Price = b.Bike.Price,
                OwnerName = b.Bike.OwnerName,
                OwnerPhone = b.Bike.OwnerPhone,
                BikeStatus = b.Bike.BikeStatus,
                Description = b.Bike.Description,
                CreatedAt = b.Bike.CreatedAt,
                ModifiedAt = b.Bike.ModifiedAt
            }).ToList();

            var images = _context.ImageCategories.ToList();

            var viewBikesModel = new BikesViewModel
            {
                Bikes = bikesSearch,
                Images = images
            };

            if (bikesSearch.Count == 0)
            {
                return Json(new { error = "Không tìm thấy dữ liệu!" });
            }
            else
            {
                return Json(viewBikesModel);
            }
        }

        public JsonResult GetLocations()
        {
            // Lấy các quận có ParentId là '01'
            var districts = _context.Locations.Where(l => l.ParentId == "01").ToList();
            var bikes = _context.Bikes.ToList();

            var viewModel = new ViewAddress
            {
                Locations = districts,
                Bikes = bikes
            };

            return Json(viewModel);
        }
    }
}
