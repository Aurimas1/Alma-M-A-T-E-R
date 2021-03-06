using System.Collections.Generic;
using System.Threading.Tasks;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, Organiser")]
    public class ApartmentController : ControllerBase
    {
        private readonly IOfficeApartmentService service;
        private readonly IApartmentService apartmentService;

        public ApartmentController(IOfficeApartmentService service, IApartmentService apartmentService)
        {
            this.service = service;
            this.apartmentService = apartmentService;
        }
        
        // GET api/apartment
        [HttpGet]
        public IEnumerable<Apartment> Get()
        {
            return service.GetAll();
        }
        
        // GET api/apartment/officeApartments
        [HttpGet]
        [Route("officeApartments")]
        [Authorize(Roles = "Admin")]
        public IEnumerable<OfficeAndApartmentsDTO> GetOfficeApartments()
        {
            return service.GetAllOfficeApartments();
        }
        
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateApartment([FromBody]Apartment apartment)
        {
            //exception handling, optimistic locking
            try
            {
                service.UpdateApartment(apartment);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict("Optimisting locking: version values are not the same.");
            }
            return Ok();
        }

        // put api/apartment/hotel/{id}
        [HttpPut("hotel/{id}")]
        public Apartment Put([FromBody]Hotel item, int id)
        {
            var g = apartmentService.Get(id);
            g.Currency = item.Currency;
            g.Price = item.Price;
            g.Address = item.Address;
            g.Name = item.Name;
            g.RoomNumber = item.RoomNumber;
            var reservation = g.Reservations.FirstOrDefault();
            reservation.CheckIn = item.CheckIn;
            reservation.CheckOut = item.CheckOut;
            return apartmentService.Update(g);
        }

        // put api/apartment/home/{id}
        [HttpPut("home/{id}")]
        public Apartment Put([FromBody]Home item, int id)
        {
            var g = apartmentService.Get(id);
            g.Address = item.Address;
            var reservation = g.Reservations.FirstOrDefault();
            reservation.CheckIn = item.CheckIn;
            reservation.CheckOut = item.CheckOut;
            return apartmentService.Update(g);
        }
        
        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "Admin")]
        public void DeleteApartment(int id)
        {
            service.DeleteApartment(id);
        }
        
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateApartment([FromBody]Apartment apartment)
        {
            await service.CreateApartment(apartment);
            return Ok();
        }

        // GET api/apartment/TripID
        [HttpGet]
        [Route("{id}")]
        public IDictionary<int, FreeRooms> GetApartamentOccupationByTripID(int id)
        {
            return service.GetApartamentOccupationByTrip(id);
        }
    }
}