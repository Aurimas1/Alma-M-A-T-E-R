using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
        
        // GET api/employee/currentUser
        [HttpGet]
        [Route("currentUser")]
        public string GetCurrentUser()
        {
            if (User.Identity.IsAuthenticated)
                return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(User.Identity.GetUserName()) + ";" +
                       (User.IsInRole(Role.Admin) ? Role.Admin : User.IsInRole(Role.Organiser) ? Role.Organiser : "User");
            return null;
        }
        
        // POST api/employee/update
        [HttpPost]
        [Route("update")]
        public void UpdateEmployees([FromBody]EmployeeRolesDTO[] employees)
        {
            service.UpdateEmployees(employees.ToList());
        }
    }

    public class EmployeeRolesDTO
    {
        public int EmployeeId { get; set; }
        public string EmployeeRole { get; set; }
    }
}
