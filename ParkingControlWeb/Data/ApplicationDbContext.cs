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

            /*            builder.Entity<IdentityRole>().HasData(new IdentityRole() { Id = Guid.NewGuid().ToString(), Name = Role.GlobalAdmin, NormalizedName = Role.GlobalAdmin.ToUpper() });
                        builder.Entity<IdentityRole>().HasData(new IdentityRole() { Id = Guid.NewGuid().ToString(), Name = Role.SystemAdmin, NormalizedName = Role.SystemAdmin.ToUpper() });
                        builder.Entity<IdentityRole>().HasData(new IdentityRole() { Id = Guid.NewGuid().ToString(), Name = Role.Expert, NormalizedName = Role.Expert.ToUpper() });
                        builder.Entity<IdentityRole>().HasData(new IdentityRole() { Id = Guid.NewGuid().ToString(), Name = Role.Driver , NormalizedName = Role.Driver.ToUpper() });
            */
            builder.Entity<MetaData>().HasData(new MetaData() { Id = Guid.NewGuid().ToString(), Key = "RenewalCardName", Value = "کامیاب محمدی تبار" });
            builder.Entity<MetaData>().HasData(new MetaData() { Id = Guid.NewGuid().ToString(), Key = "RenewalCardNumber", Value = "585983119387" });

        }

        public DbSet<Info> Info { get; set; }
        public DbSet<Parking> Parkings { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Pricing> Pricings { get; set; }
        public DbSet<Ballance> Ballances { get; set; }
        public DbSet<RenewalRequest> RenewalRequests { get; set; }
        public DbSet<MetaData> MetaDatas { get; set; }
    }
}
