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

            builder.Entity<IdentityRole>().HasData(new IdentityRole() { Id = Guid.NewGuid().ToString(), Name = Role.GlobalAdmin, NormalizedName = Role.GlobalAdmin.ToUpper() });
            builder.Entity<IdentityRole>().HasData(new IdentityRole() { Id = Guid.NewGuid().ToString(), Name = Role.SystemAdmin, NormalizedName = Role.SystemAdmin.ToUpper() });
            builder.Entity<IdentityRole>().HasData(new IdentityRole() { Id = Guid.NewGuid().ToString(), Name = Role.Expert, NormalizedName = Role.Expert.ToUpper() });
            builder.Entity<IdentityRole>().HasData(new IdentityRole() { Id = Guid.NewGuid().ToString(), Name = Role.Driver , NormalizedName = Role.Driver.ToUpper() });
        }

        public DbSet<Info> Info { get; set; }
        public DbSet<Parking> Parkings { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

    }
}
