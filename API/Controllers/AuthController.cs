using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Net.Http;
using System.Security.Claims;
using System.Net.Http.Formatting;
using API.Services;

namespace API.Controllers
{
    [Authorize]
    public class AuthController : Controller
    {
        private readonly IEmployeeService service;
        private readonly IGoogleCalendarService calendarService;
        private readonly IEventService eventService;

        public AuthController(IEmployeeService service, IGoogleCalendarService calendarService, IEventService eventService)
        {
            this.service = service;
            this.calendarService = calendarService;
            this.eventService = eventService;
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
            return Redirect("/login.html");
        }

        [Route("cb")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> EnsureUser([FromBody] EnsureParams parameters)
        {
            var user = await service.Ensure(parameters.Email, parameters.Name);
            var events = await calendarService.GetEvents(parameters.AccessToken);

            //await eventService.SaveEventsForEmployee(events.Items.ToEvents(user.EmployeeID));

            return Ok(new CallbackResult
            {
                Role = user.Role,
                Id = user.EmployeeID,
            });
        }

        public static async Task Callback(OAuthCreatingTicketContext context)
        {
            var data = new EnsureParams
            {
                Name = context.Identity.Name,
                Email = context.Identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                AccessToken = context.AccessToken,
            };
            using (var req = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44313/cb"))
            {
                req.Content = new ObjectContent<EnsureParams>(data, new JsonMediaTypeFormatter());
                var response = await context.Backchannel.SendAsync(req);
                var result = await response.Content.ReadAsAsync<CallbackResult>();
                if (result.Role != null)
                {
                    context.Identity.AddClaim(new Claim(ClaimTypes.Role, result.Role));
                }
                context.Identity.AddClaim(new Claim(CustomClaimTypes.EmployeeID, result.Id.ToString()));
            }
        }
    }

    public class EnsureParams
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string AccessToken { get; set; }
    }

    public class CallbackResult
    {
        public string Role { get; set; }
        public int Id { get; set; }
    }
}
