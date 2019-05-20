using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using API.Repositories;
using API.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Organiser")]
    public class AirplaneController : ControllerBase
    {
        private readonly IRepository<PlaneTicket> planeTicketRepository;

        public AirplaneController(IRepository<PlaneTicket> planeTicketRepository)
        {
            this.planeTicketRepository = planeTicketRepository;
        }

        // DELETE api/airplane/{id}
        [HttpDelete]
        [Route("{id}")]
        public ActionResult Delete(int id)
        {
            if (planeTicketRepository.Delete(id))
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        // put api/airplane
        [HttpPut]
        public PlaneTicket Put([FromBody]PlaneTicket item)
        {
            var g = planeTicketRepository.Get(item.PlaneTicketID);
            g.Currency = item.Currency;
            g.Price = item.Price;
            g.PlaneTicketUrl = item.PlaneTicketUrl;
            g.ReturnFlightDate = item.ReturnFlightDate;
            g.ForwardFlightDate = item.ForwardFlightDate;
            g.Airport = item.Airport;
            g.FlightCompany = item.FlightCompany;
            return planeTicketRepository.Update(g);
        }

    }
}
