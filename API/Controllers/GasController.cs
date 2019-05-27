using API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Organiser")]
    public class GasController : ControllerBase
    {
        private readonly IRepository<GasCompensation> gasCompensationRepository;

        public GasController(IRepository<GasCompensation> gasCompensationRepository)
        {
            this.gasCompensationRepository = gasCompensationRepository;
        }

        // DELETE api/gas/{id}
        [HttpDelete]
        [Route("{id}")]
        public ActionResult Delete(int id)
        {
            if (gasCompensationRepository.Delete(id))
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        // put api/gas
        [HttpPut]
        public GasCompensation Put([FromBody]GasCompensation item)
        {
            var g = gasCompensationRepository.Get(item.GasCompensationID);
            g.EmployeeID = item.EmployeeID;
            g.Currency = item.Currency;
            g.Price = item.Price;
            return gasCompensationRepository.Update(g);
        }

    }
}
