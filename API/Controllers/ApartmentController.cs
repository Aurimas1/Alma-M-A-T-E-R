using System.Collections.Generic;
using API.Constants;
using API.Services;
using Microsoft.AspNetCore.Mvc;
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

        // GET api/apartment/OfficeID
        [HttpGet]
        [Route("{id}")]
        public IDictionary<int, FreeRooms> GetApartamentOccupationByOfficeID(int id, DateTime from, DateTime to)
        {
            return service.GetApartamentOccupationByOffice(id, from, to);
        }
    }
}