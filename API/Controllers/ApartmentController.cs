using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using API.Constants;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize(Roles = "Admin, Organiser")]
    public class ApartmentController : ControllerBase
    {
        private readonly IOfficeApartmentService service;

        public ApartmentController(IOfficeApartmentService service)
        {
            this.service = service;
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
        public IEnumerable<OfficeAndApartmentsDTO> GetCurrentUser()
        {
            return service.GetAllOfficeApartments();
        }
        
        [HttpPut]
        public IActionResult UpdateApartment([FromBody]Apartment apartment)
        {
            //exception handling, optimistic locking
            try
            {
                service.UpdateApartment(apartment);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Conflict("Optimisting locking: version values are not the same.");
            }
            return Ok();
        }
        
        [HttpDelete]
        public void DeleteApartment([FromBody]int id)
        {
            service.DeleteApartment(id);
        }
        
        [HttpPost]
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