using System.Security.Claims;

namespace ParkingControlWeb.Data
{
    public static class ClaimPrincipalExtension
    {

        public static string GetUserId(this ClaimsPrincipal claim) =>
            claim.FindFirst(ClaimTypes.NameIdentifier).Value;
    }
}
