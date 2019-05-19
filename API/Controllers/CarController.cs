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
    public class CarController : ControllerBase
    {
        private readonly IRepository<CarRental> carRentalRepository;

        public CarController(IRepository<CarRental> carRentalRepository)
        {
            this.carRentalRepository = carRentalRepository;
        }

        // DELETE api/car/{id}
        [HttpDelete]
        [Route("{id}")]
        public ActionResult Delete(int id)
        {
            if (carRentalRepository.Delete(id))
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
        public CarRental Put([FromBody]CarRental item)
        {
            var g = carRentalRepository.Get(item.CarRentalID);
            g.CarPickupAddress = item.CarPickupAddress;
            g.Currency = item.Currency;
            g.Price = item.Price;
            g.CarRentalCompany = item.CarRentalCompany;
            g.CarRentalUrl = item.CarRentalUrl;
            g.CarIssueDate = item.CarIssueDate;
            g.CarReturnDate = item.CarReturnDate;
            return carRentalRepository.Update(g);
        }

    }
}
