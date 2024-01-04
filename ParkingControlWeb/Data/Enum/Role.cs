using Microsoft.AspNetCore.Identity;

namespace ParkingControlWeb.Data.Enum
{
    public class Role : IdentityRole
    {
        public static readonly string GlobalAdmin = "GlobalAdmin";
        public static readonly string SystemAdmin = "SystemAdmin";
        public static readonly string Expert = "Expert";
        public static readonly string Driver = "Driver";
    }
}
