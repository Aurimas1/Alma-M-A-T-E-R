using System.Collections.Generic;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    //[Authorize(Roles = "Kebab")]
    public class TripController : ControllerBase
    {
        private readonly ITripService service;

        public TripController(ITripService service)
        {
            this.service = service;
        }

        // GET api/Trip
        [HttpGet]
        public IEnumerable<Trip> Get()
        {
            return service.GetAll();
        }

        // GET api/Trip/employees
        [Route("employees")]
        [HttpGet]
        public IEnumerable<Employee> GetEmployeesFromTrip(int id)
        {
            return service.GetEmployeesFromTrip(id);
        }
    }
}
