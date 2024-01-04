using Microsoft.AspNetCore.Identity;

namespace ParkingControlWeb.Data.Enum
{
    public class Role : IdentityRole
    {
        public const string GlobalAdmin = "GlobalAdmin";
        public const string SystemAdmin = "SystemAdmin";
        public const string Expert = "Expert";
        public const string Driver = "Driver";
    }
}
