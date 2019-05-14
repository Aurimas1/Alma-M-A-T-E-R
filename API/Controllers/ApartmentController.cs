using System.Collections.Generic;
using System.Threading.Tasks;
using API.Constants;
using API.Services;
using Microsoft.AspNetCore.Mvc;

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
        
        // POST api/apartment/update
        [HttpPost]
        [Route("update")]
        public void UpdateApartment([FromBody]Apartment apartment)
        {
            //exception handling, optimistic locking needed
            service.UpdateApartment(apartment);
        }
        
        // POST api/apartment/delete
        [HttpPost]
        [Route("delete")]
        public void DeleteApartment([FromBody]int id)
        {
            service.DeleteApartment(id);
        }
        
        // POST api/apartment/create
        [HttpPost]
        [Route("create")]
        public async Task<OkResult> CreateApartment([FromBody]Apartment apartment)
        {
            await service.CreateApartment(apartment);
            return Ok();
        }
        
    }
}