using BikeTogether.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BikeTogether.Controllers
{
    public class AdminController : Controller
    {
        private readonly ProjectDbContext _context;

        public AdminController(ProjectDbContext context)
        {
            _context = context;
        }

        public IActionResult BikeManage()
        {
            if(HttpContext.Session.GetString("Role") == "role1")
            {
                return View();
            }
            else
            {
                return RedirectToAction("UserInfo", "User");
            }
        }


        //TreeView=======================================================================================
        [HttpPost]
        public JsonResult RootsTreeView()
        {

            List<Location> roots = new List<Location>();
            roots = _context.Locations.Where(a => a.ParentId.Equals("#")).OrderBy(a => a.Name).ToList();
            return Json(roots);
        }

        [HttpPost]
        public JsonResult GetTreeView(string pid)
        {
            List<Location> subNodes = new List<Location>();

            subNodes = _context.Locations.
                Where(a => a.ParentId.Equals(pid)).OrderBy(a => a.Name).ToList();

            return Json(subNodes);
        }


        //DataTable=====================================================================================
        [HttpPost]
        public string getDataTables(string locationId)
        {
            List<Bike> listBike = _context.Bikes.Where(a => a.OfLocationId.Contains(locationId)).ToList();
            var value = JsonConvert.SerializeObject(new { data = listBike });
            return value;
        }


        //Thêm mới=======================================================================================
        public class BikeAndImageCategory
        {
            public Bike bikeObject { get; set; }
            public ImageCategory imageCategoryObject { get; set; }
        }

        [HttpPost]
        public async Task<int> ThemMoi([FromBody] BikeAndImageCategory bikeAndImageCategory)
        {
            var result = -1;
            try
            {
                _context.Bikes.Add(bikeAndImageCategory.bikeObject);

                var bikesId = bikeAndImageCategory.bikeObject.Id;

                var urls = bikeAndImageCategory.imageCategoryObject.Url;
                if (urls.EndsWith(";"))
                {
                    urls = urls.Remove(urls.Length - 1);
                }

                foreach (var url in urls.Split(';'))
                {
                    var imageCategory = new ImageCategory
                    {
                        BikesId = bikesId,
                        Url = url
                    };
                    _context.ImageCategories.Add(imageCategory);
                }

                await _context.SaveChangesAsync();
                result = 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                result = 0;
            }
            return result;
        }



        // Chi tiết======================================================================================
        public class ListBikeAndImageCategory
        {
            public List<Bike> Bikes { get; set; }
            public List<ImageCategory> ImageCategories { get; set; }
        }
        [HttpGet]
        public JsonResult ChiTiet(Guid bikeId)
        {
            var data = new ListBikeAndImageCategory();

            data.Bikes = _context.Bikes.
                Where(a => a.Id.Equals(bikeId)).ToList();
            data.ImageCategories = _context.ImageCategories.
                Where(a => a.BikesId.Equals(bikeId)).ToList();

            return Json(data);
        }



        // Cập nhật========================================================================================
        [HttpPost]
        public async Task<int> CapNhat([FromBody] BikeAndImageCategory bikeAndImageCategory)
        {
            var result = -1;

            try
            {
                _context.Bikes.Update(bikeAndImageCategory.bikeObject);
                var imageCategoriesToDelete = _context.ImageCategories.Where(ic => ic.BikesId == bikeAndImageCategory.bikeObject.Id);
                _context.ImageCategories.RemoveRange(imageCategoriesToDelete);

                var bikeId = bikeAndImageCategory.bikeObject.Id;

                var urls = bikeAndImageCategory.imageCategoryObject.Url;
                if (urls.EndsWith(";"))
                {
                    urls = urls.Remove(urls.Length - 1);
                }

                foreach (var url in urls.Split(';'))
                {
                    var imageCategory = new ImageCategory
                    {
                        BikesId = bikeId,
                        Url = url
                    };
                    _context.ImageCategories.Add(imageCategory);
                }

                //_context.ImageCategories.Update(houseAndImageCategory.imageCategoryObject);
                await _context.SaveChangesAsync();
                result = 1;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BikeExists(bikeAndImageCategory.bikeObject.Id))
                {
                    result = 0;
                }
                else
                {
                    throw;
                }
            }
            return result;
        }

        // Xóa=====================================================================================================
        [HttpPost]

        public async Task<int> DeleteConfirmed(Guid bikeId)
        {
            var result = -1;
            var bike = await _context.Bikes.FindAsync(bikeId);
            if (bike != null)
            {
                _context.Bikes.Remove(bike);
                var imageCategoriesToDelete = _context.ImageCategories.Where(ic => ic.BikesId == bikeId);
                _context.ImageCategories.RemoveRange(imageCategoriesToDelete);
                await _context.SaveChangesAsync();
                result = 1;
            }
            else
            {
                result = 0;
            }

            return result;
        }


        private bool BikeExists(Guid bikeId)
        {
            return (_context.Bikes?.Any(e => e.Id == bikeId)).GetValueOrDefault();
        }
    }
}
