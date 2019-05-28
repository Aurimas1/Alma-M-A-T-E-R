using System.Linq;
using System.Security.Claims;

namespace API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetEmpoeeID(this ClaimsPrincipal user)
        {
            return int.Parse(user.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.EmployeeID).Value);
        }

        public static string GetRole(this ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
        }
    }
}
