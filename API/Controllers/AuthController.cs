using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Net.Http;
using System.Security.Claims;
using System.Net.Http.Formatting;

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

        [Route("cb")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser([FromBody] ClaimsIdentity identity = null)
        {
            return Ok();
        }

        public static async Task Callback(OAuthCreatingTicketContext context)
        {
            
            //using (var req = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44313/cb")) {
            //    //req.Content = new ObjectContent<ClaimsIdentity>(context.Identity, new JsonMediaTypeFormatter());
            //   // var response = await context.Backchannel.SendAsync(req);
            //}
        }
    }
}
