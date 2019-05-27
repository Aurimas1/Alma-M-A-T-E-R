using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Repositories;
using API.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReservationController : ControllerBase
    {
        private readonly IRepository<Reservation> reservationRepository;
        private readonly IRepository<Apartment> apartmentRepository;
        private readonly IRepository<EmployeeToTrip> employeeTotripRepository;

        public ReservationController(IRepository<Reservation> reservationRepository, IRepository<Apartment> apartmentRepository, IRepository<EmployeeToTrip> employeeTotripRepository)
        {
            this.reservationRepository = reservationRepository;
            this.apartmentRepository = apartmentRepository;
            this.employeeTotripRepository = employeeTotripRepository;
        }

        // DELETE api/reservation/{id}
        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "Admin,Organiser")]
        public ActionResult Delete(int id)
        {
            var apartment = reservationRepository.Get(id).Apartment;
            if (reservationRepository.Delete(id))
            {
                if (apartment.Type != "OFFICE") apartmentRepository.Delete(apartment.ApartmentID);
                return Ok();
            }
            else
            {
                return NotFound();
            }

        }

        // POST api/reservation/setHome/{id}
        [HttpPost]
        [Route("setHome/{id}")]
        public async Task<ActionResult> SetHome(int id)
        {
            var employeeToTrip = employeeTotripRepository.Get(id);
            var reservation = reservationRepository.GetAll().Where(x => x.EmployeeID == employeeToTrip.EmployeeID && x.TripID == employeeToTrip.TripId);

            if (reservation.Any())
            {
                var apartment = apartmentRepository.Get(reservation.First().ApartmentID);
                if (apartment.Type != "OFFICE") apartmentRepository.Delete(apartment.ApartmentID);
                //reservationRepository.Delete(reservation.First().ReservationID);
            }

            var home = new Apartment
            {
                Name = "HOME",
                RoomNumber = 1,
                Price = 0,
                Currency = "EUR",
                Address = " ",
                Type = "HOME",
                OfficeId = employeeToTrip.Trip.ArrivalOfficeID
            };

            await apartmentRepository.Add(home);

            var newReservation = new Reservation
            {
                TripID = employeeToTrip.TripId,
                EmployeeID = employeeToTrip.EmployeeID,
                ApartmentID = home.ApartmentID,
                CheckIn = employeeToTrip.Trip.DepartureDate,
                CheckOut = employeeToTrip.Trip.ReturnDate
            };

            await reservationRepository.Add(newReservation);
            return Ok();

        }
    }
}
