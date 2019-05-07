using System.Collections.Generic;
using System.Threading.Tasks;
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
        private readonly IEmployeeToTripService employeeToTripService;

        public TripController(ITripService service, IEmployeeToTripService employeeToTripService)
        {
            this.service = service;
            this.employeeToTripService = employeeToTripService;
        }

        // POST api/Trip/saveTrip
        [Route("saveTrip")]
        [HttpPost]
        public async Task<Trip> Add([FromBody]CreateTrip item)
        {
            var trip = new Trip
            {
                ArrivalOfficeID = item.ArrivalOfficeID,
                DepartureDate = item.DepartureDate,
                DepartureOfficeID = item.DepartureOfficeID,
                IsPlaneNeeded = item.IsPlaneNeeded,
                IsCarCompensationNeeded = item.IsCarCompensationNeeded,
                IsCarRentalNeeded = item.IsCarRentalNeeded,
                ReturnDate = item.ReturnDate,
                Status = "CREATED",
            };
            var result = await service.Add(trip);

            foreach(var employee in item.Employees)
            {
                var employeeToTrip = new EmployeeToTrip
                {
                    EmployeeID = employee,
                    TripId = result.TripID,
                    Status = "PENDING",
                    WasRead = false,
                };
                await employeeToTripService.Add(employeeToTrip);
            }

            return result;
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
