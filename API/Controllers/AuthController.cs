using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace API.Controllers
{
	public class AuthController : Controller
    {
		[Authorize]
		[Route("secret")]
		public IActionResult Secret()
		{
			return View(new User(this.User));
		}

		[Route("home")]
		public IActionResult Home()
		{
			return View(this.User.Identities.Any(v=>v.IsAuthenticated));
		}
	}

    public class User
    {
        private ClaimsPrincipal principal;

        public User(ClaimsPrincipal principal)
        {
            this.principal = principal;
        }
        public string Name => principal.Identity.Name;
        public string Email => principal.FindFirst(ClaimTypes.Email).Value;
    }
}
