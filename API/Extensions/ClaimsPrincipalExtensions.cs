using System.Linq;
using System.Security.Claims;

namespace API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetEmpoeeID(this ClaimsPrincipal user)
        {
            return int.Parse(user.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.EmpoeeID).Value);
        }
    }
}
