using System.Collections.Generic;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Organiser")]
    public class OfficeController : ControllerBase
    {
        private readonly IOfficeService service;

        public OfficeController(IOfficeService service)
        {
            this.service = service;
        }

        // GET api/office
        [HttpGet]
        public IEnumerable<Office> Get()
        {
            return service.GetAll();
        }
    }
}
