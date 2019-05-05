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
        public IEnumerable<Employee> GetEmployees(int id)
        {
            return service.GetEmployees(id);
        }

        // GET api/Trip/reservedApartments
        [Route("reservedApartments")]
        [HttpGet]
        public IEnumerable<Apartment> GetReservedApartments(int id)
        {
            return service.GetReservedApartments(id);
        }

        // GET api/Trip/planeTickets
        [Route("planeTickets")]
        [HttpGet]
        public IEnumerable<PlaneTicket> GetPlaneTickets(int id)
        {
            return service.GetPlaneTickets(id);
        }

        // GET api/Trip/carRentals
        [Route("carRentals")]
        [HttpGet]
        public IEnumerable<CarRental> GetCarRentals(int id)
        {
            return service.GetCarRentals(id);
        }

        // GET api/Trip/gasCompensations
        [Route("gasCompensations")]
        [HttpGet]
        public IEnumerable<GasCompensation> GetGasCompensations(int id)
        {
            return service.GetGasCompensations(id);
        }
    }
}
