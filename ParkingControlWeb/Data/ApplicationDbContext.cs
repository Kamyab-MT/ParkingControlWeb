using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ParkingControlWeb.Data.Enum;
using ParkingControlWeb.Models;

namespace ParkingControlWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser, Role, string>
    {

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Info> Info { get; set; }
        public DbSet<Parking> Parkings { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Record> Records { get; set; }

    }
}
