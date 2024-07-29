using Microsoft.EntityFrameworkCore;
namespace BikeTogether.Models
{
    public class ProjectDbContext : DbContext
    {
        public ProjectDbContext(DbContextOptions options) : base(options)
        {
        }
        public virtual DbSet<Bike> Bikes { get; set; }
        public virtual DbSet<ImageCategory> ImageCategories { get; set; }
        public virtual DbSet<BookingBike> BookingBikes { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Location> Locations { get; set; }

    }
}
