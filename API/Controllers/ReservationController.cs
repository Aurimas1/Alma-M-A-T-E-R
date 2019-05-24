using API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Organiser")]
    public class ReservationController : ControllerBase
    {
        private readonly IRepository<Reservation> reservationRepository;
        private readonly IRepository<Apartment> apartmentRepository;

        public ReservationController(IRepository<Reservation> reservationRepository, IRepository<Apartment> apartmentRepository)
        {
            this.reservationRepository = reservationRepository;
            this.apartmentRepository = apartmentRepository;
        }

        // DELETE api/reservation/{id}
        [HttpDelete]
        [Route("{id}")]
        public ActionResult Delete(int id)
        {
            var apartment = reservationRepository.Get(id).Apartment;
            if (reservationRepository.Delete(id))
            {
                if(apartment.Type != "OFFICE")apartmentRepository.Delete(apartment.ApartmentID);
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

    }
}
