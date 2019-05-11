using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
using API.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    //[Authorize(Roles = "Kebab")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService service;

        public EmployeeController(IEmployeeService service)
        {
            this.service = service;
        }

        // GET api/employee
        [HttpGet]
        public IEnumerable<Employee> Get()
        {
            return service.GetAll();
        }
        
        // POST api/employee/currentUser
        [HttpGet]
        [Route("currentUser")]
        public string GetCurrentUser()
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(User.Identity.GetUserName()) + ";" + (User.IsInRole("Admin")?"Admin":"") + (User.IsInRole("Organiser")?"Organiser":"User");
        }
    }
}
