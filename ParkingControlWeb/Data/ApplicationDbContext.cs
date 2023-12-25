using Microsoft.EntityFrameworkCore;
using ParkingControlWeb.Models;

namespace ParkingControlWeb.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Info> Info { get; set; }
        public DbSet<Parking> Parkings { get; set; }
        public DbSet<Car> Cars { get; set; }
    }
}
