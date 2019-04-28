using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Net.Http;
using System.Security.Claims;
using System.Net.Http.Formatting;
using System.Linq;
using API.Services;

namespace API.Controllers
{
    [Authorize]
    public class AuthController : Controller
    {
        private readonly IEmployeeService service;

        public AuthController(IEmployeeService service)
        {
            this.service = service;
        }

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
        public async Task<IActionResult> EnsureUser([FromBody] Employee user = null)
        {
            user = await service.Ensure(user);

            return Ok(user.Role);
        }

        public static async Task Callback(OAuthCreatingTicketContext context)
        {
            var data = new Employee
            {
                Name = context.Identity.Name,
                Email = context.Identity.Claims.Where(x => x.Type == ClaimTypes.Email).Single().Value,
            };
            using (var req = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44313/cb"))
            {
                req.Content = new ObjectContent<Employee>(data, new JsonMediaTypeFormatter());
                var response = await context.Backchannel.SendAsync(req);
                context.Identity.AddClaim(new Claim(ClaimTypes.Role, await response.Content.ReadAsStringAsync()));
            }
        }
    }
}
