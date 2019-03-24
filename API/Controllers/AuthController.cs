using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

namespace API.Controllers
{
    [Authorize]
    public class AuthController : Controller
    {
		[Route("login")]
		public IActionResult Login()
		{
            return Redirect("/index.html");
		}

		[Route("logout")]
		public async Task<IActionResult> Logout()
		{
            await HttpContext.SignOutAsync();
            foreach (var cookie in HttpContext.Request.Cookies) //nice thing to do, but SignOutAsync it is all you need
                HttpContext.Response.Cookies.Delete(cookie.Key);
			return Redirect("/login");
		}
	}
}
