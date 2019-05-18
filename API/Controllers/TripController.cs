using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Extensions;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    //[Authorize(Roles = "Kebab")]
    public class TripController : ControllerBase
    {
        private readonly ITripService service;
        private readonly IEmployeeToTripService employeeToTripService;
        private readonly IApartmentService apartmentService;

        public TripController(ITripService service, IEmployeeToTripService employeeToTripService, IApartmentService apartmentService)
        {
            this.service = service;
            this.employeeToTripService = employeeToTripService;
            this.apartmentService = apartmentService;
        }

        // POST api/Trip
        [HttpPost]
        public async Task<ActionResult> Add([FromBody]CreateTrip item)
        {
            var trip = new Trip
            {
                ArrivalOfficeID = item.ArrivalOfficeID,
                DepartureDate = item.DepartureDate,
                DepartureOfficeID = item.DepartureOfficeID,
                IsPlaneNeeded = item.IsPlaneNeeded,
                IsCarCompensationNeeded = item.IsCarCompensationNeeded,
                IsCarRentalNeeded = item.IsCarRentalNeeded,
                ReturnDate = item.ReturnDate,
                Status = "CREATED",
                OrganizerID = User.GetEmpoeeID(),
            };
            var result = await service.Add(trip);

            foreach (var employee in item.Employees)
            {
                var employeeToTrip = new EmployeeToTrip
                {
                    EmployeeID = employee,
                    TripId = result.TripID,
                    Status = "PENDING",
                    WasRead = false,
                };
                await employeeToTripService.Add(employeeToTrip);
            }

            return Ok();
        }

        // PUT api/Trip/{id}
        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> Update([FromBody]UpdateTrip item, int id)
        {
            Trip trip = service.GetByID(id);
            trip.IsCarCompensationNeeded = item.IsCarCompensationNeeded;
            trip.IsPlaneNeeded = item.IsPlaneNeeded;
            trip.IsCarRentalNeeded = item.IsCarRentalNeeded;
            foreach (var employee in item.Employees)
            {
                if (trip.EmployeesToTrip?.Any(x => x.EmployeeID == employee) == false)
                {
                    var employeeToTrip = new EmployeeToTrip
                    {
                        EmployeeID = employee,
                        TripId = id,
                        Status = "PENDING",
                        WasRead = false,
                    };
                    await employeeToTripService.Add(employeeToTrip);
                }
            }

            var idsToDelete = new List<int>();
            var usersInfoToDelete = new List<int>();
            foreach (var e in trip.EmployeesToTrip)
            {
                if (!item.Employees.Contains(e.EmployeeID))
                {
                    idsToDelete.Add(e.EmployeeToTripID);
                    usersInfoToDelete.Add(e.EmployeeID);
                    //employeeToTripService.Remove(e.EmployeeToTripID);
                }
            }

            foreach (var idToDelete in idsToDelete)
            {
                employeeToTripService.Remove(idToDelete);
            }

            var tickets = trip.PlaneTickets.Where(x => usersInfoToDelete.Contains(x.EmployeeID));
            var gas = trip.GasCompensations.Where(x => usersInfoToDelete.Contains(x.EmployeeID));
            var reservations = trip.Reservations.Where(x => usersInfoToDelete.Contains(x.EmployeeID));

            trip.Reservations = item.Rooms.Select(x => new Reservation
            {
                ApartmentID = trip.ArrivalOffice.Apartaments.First(a => a.RoomNumber == x.RoomID).ApartmentID,
                EmployeeID = x.EmployeeID,
                CheckIn = trip.DepartureDate,
                CheckOut = trip.ReturnDate,
                TripID = trip.TripID,
                ReservationUrl = "-",
            }).ToList();

            trip.PlaneTickets = trip.PlaneTickets.Where(x => !tickets.Select(y => y.PlaneTicketID).Contains(x.PlaneTicketID)).ToList();
            trip.GasCompensations = trip.GasCompensations.Where(x => !gas.Select(y => y.GasCompensationID).Contains(x.GasCompensationID)).ToList();

            var result = service.Update(trip);

            return Ok();
        }

        // Post api/Trip/gasCompensation
        [Route("gasCompensation")]
        [HttpPost]
        public async Task<ActionResult> AddGasCompensation([FromBody]GasCompensation item)
        {
            await service.SaveGasCompensation(new GasCompensation()
            {
                TripID = item.TripID,
                EmployeeID = item.EmployeeID,
                Price = item.Price,
                Currency = item.Currency,
            });

            return Ok();
        }

        // Post api/Trip/carRental
        [Route("carRental")]
        [HttpPost]
        public async Task<ActionResult> AddCarRental([FromBody]CarRental item)
        {
            await service.SaveCarRental(new CarRental()
            {
                TripID = item.TripID,
                Price = item.Price,
                CarRentalCompany = item.CarRentalCompany,
                CarPickupAddress = item.CarPickupAddress,
                CarRentalUrl = item.CarRentalUrl,
                CarIssueDate = item.CarIssueDate,
                CarReturnDate = item.CarReturnDate,
                Currency = item.Currency,
            });

            return Ok();
        }

        // Post api/Trip/planeTicket
        [Route("planeTicket")]
        [HttpPost]
        public async Task<ActionResult> AddPlaneTicket([FromBody]PlaneTicket item)
        {
            await service.SavePlaneTicket(new PlaneTicket()
            {
                TripID = item.TripID,
                Price = item.Price,
                PlaneTicketUrl = item.PlaneTicketUrl,
                ForwardFlightDate = item.ForwardFlightDate,
                ReturnFlightDate = item.ReturnFlightDate,
                Airport = item.Airport,
                FlightCompany = item.FlightCompany,
                EmployeeID = item.EmployeeID,
                Currency = item.Currency,
            });

            return Ok();
        }

        // Post api/Trip/hotel
        [Route("hotel")]
        [HttpPost]
        public async Task<ActionResult> AddHouse([FromBody]Hotel item)
        {
            var apartament = await service.SaveHotelorHome(new Apartment()
            {
                OfficeId = null,
                Address = item.Address,
                Name = item.Name,
                Price = item.Price,
                RoomNumber = item.RoomNumber,
                Type = "HOTEL",
                Currency = item.Currency,
            });
            await service.SaveReservation(new Reservation()
            {
                ApartmentID = apartament.ApartmentID,
                CheckIn = item.CheckIn,
                CheckOut = item.CheckOut,
                ReservationUrl = item.ReservationUrl,
                TripID = item.TripID,
                EmployeeID = item.EmployeeID,
            });
            return Ok();
        }

        // Post api/Trip/home
        [Route("home")]
        [HttpPost]
        public async Task<ActionResult> AddHome([FromBody]Home item)
        {
            var apartament = await service.SaveHotelorHome(new Apartment()
            {
                OfficeId = null,
                Address = item.Address,
                Name = null,
                Price = 0,
                RoomNumber = 0,
                Type = "HOME",
                Currency = item.Currency,
            });
            await service.SaveReservation(new Reservation()
            {
                ApartmentID = apartament.ApartmentID,
                CheckIn = item.CheckIn,
                CheckOut = item.CheckOut,
                ReservationUrl = null,
                TripID = item.TripID,
                EmployeeID = item.EmployeeID,
            });
            return Ok();
        }

        // GET api/Trip
        [HttpGet]
        public IEnumerable<TripFilter> Get()
        {
            return service.GetAll().ToTripFilter();
        }

        // GET api/Trip
        [Route("{ID}")]
        [HttpGet]
        public object Get(int ID)
        {
            var trip = service.Get(ID);
            var apartment = apartmentService.GetAll();
            var tripToBoard = new
            {
                ArrivalCountry = trip.ArrivalOffice.Country,
                ArrivalCity = trip.ArrivalOffice.City,
                DepartureCountry = trip.DepartureOffice.Country,
                DepartureCity = trip.DepartureOffice.City,
                ReturnDate = trip.ReturnDate,
                DepartureDate = trip.DepartureDate,
                Status = trip.Status,
                EmployeeName = trip.EmployeesToTrip.Select(x => x.Employee.Name),
                EmployeeEmail = trip.EmployeesToTrip.Select(x => x.Employee.Email),
                EmployeeID = trip.EmployeesToTrip.Select(x => x.Employee.EmployeeID),

                Tickets = trip.PlaneTickets?.ToInfo(),

                trip.IsPlaneNeeded,
                Accomodation = trip.Reservations?.Select(x => x.Apartment.Name),
                Address = trip.Reservations?.Select(x => x.Apartment.Address),
                RoomNumber = trip.Reservations?.Select(x => x.Apartment.RoomNumber),
                CheckIn = trip.Reservations?.Select(x => x.CheckIn),
                CheckOut = trip.Reservations?.Select(x => x.CheckOut),
                AccomodationUrl = trip.Reservations?.Select(x => x.ReservationUrl),
                Price = trip.Reservations?.Select(x => x.Apartment.Price),
                Currency = trip.Reservations?.Select(x => x.Apartment.Currency),

                trip.IsCarRentalNeeded,
                Rentals = trip.CarRentals?.ToInfo(),

                trip.IsCarCompensationNeeded,
                GasCompensations = trip.GasCompensations?.ToInfo(),
            };
            return tripToBoard;
        }

        // GET api/Trip/employees/{id}
        [Route("employees/{id}")]
        [HttpGet]
        public IEnumerable<Employee> GetEmployees(int id)
        {
            return service.GetEmployees(id);
        }

        // GET api/Trip/Time/{id}
        [Route("time/{id}")]
        [HttpGet]
        public Times GetTimes(int id)
        {
            return service.GetTimes(id);
        }

        // GET api/Trip/reservedApartments
        [Route("reservedApartments")]
        [HttpGet]
        public IEnumerable<Apartment> GetReservedApartments(int id)
        {
            return service.GetReservedApartments(id);
        }

        // GET api/Trip/planeTickets
        [Route("planeTickets")]
        [HttpGet]
        public IEnumerable<PlaneTicket> GetPlaneTickets(int id)
        {
            return service.GetPlaneTickets(id);
        }

        // GET api/Trip/carRentals
        [Route("carRentals")]
        [HttpGet]
        public IEnumerable<CarRental> GetCarRentals(int id)
        {
            return service.GetCarRentals(id);
        }

        // GET api/Trip/gasCompensations
        [Route("gasCompensations")]
        [HttpGet]
        public IEnumerable<GasCompensation> GetGasCompensations(int id)
        {
            return service.GetGasCompensations(id);
        }

        // GET api/Trip/
        [HttpGet("filter")]
        public ActionResult<IEnumerable<TripFilter>> Get(bool tripsAwaitingConfirmation, bool tripsConfirmed, bool fullyPlannedTrips, bool finishedTrips, bool myOrganizedTrips, bool otherOrganizedTrips, string dateFrom, string dateTo)
        {
            var filter = new Filter();
            filter.TripsAwaitingConfirmation = tripsAwaitingConfirmation;
            filter.TripsConfirmed = tripsConfirmed;
            filter.FullyPlannedTrips = fullyPlannedTrips;
            filter.FinishedTrips = finishedTrips;
            filter.MyOrganizedTrips = myOrganizedTrips;
            filter.OtherOrganizedTrips = otherOrganizedTrips;
            var date = dateFrom.Split("GMT");
            filter.DateFrom = Convert.ToDateTime(date[0]);
            date = dateTo.Split("GMT");
            filter.DateTo = Convert.ToDateTime(date[0]);

            IEnumerable<Trip> trips;

            if (filter.MyOrganizedTrips && filter.OtherOrganizedTrips)
            {
                trips = service.GetAll().Where(s => s.DepartureDate >= filter.DateFrom && s.ReturnDate <= filter.DateTo);
            }
            else if (filter.MyOrganizedTrips)
            {
                trips = service.GetYourOrganizedTrips().Where(s => s.DepartureDate >= filter.DateFrom && s.ReturnDate <= filter.DateTo);
            }
            else if (filter.OtherOrganizedTrips)
            {
                trips = service.GetOtherOrganizedTrips().Where(s => s.DepartureDate >= filter.DateFrom && s.ReturnDate <= filter.DateTo);
            }
            else
            {
                return NotFound();
            }

            if (filter.TripsAwaitingConfirmation && filter.TripsConfirmed && filter.FullyPlannedTrips && filter.FinishedTrips)
            {
            }
            //THREE PARAMETERS
            else if (filter.TripsAwaitingConfirmation && filter.TripsConfirmed && filter.FullyPlannedTrips)
            {
                trips = trips.Where(s => s.Status == "CREATED" || s.Status == "CONFIRMED" || s.Status == "PLANNED");
            }
            else if (filter.TripsAwaitingConfirmation && filter.TripsConfirmed && filter.FinishedTrips)
            {
                trips = trips.Where(s => s.Status == "CREATED" || s.Status == "CONFIRMED" || s.Status == "FINISHED");
            }
            else if (filter.TripsAwaitingConfirmation && filter.FinishedTrips && filter.FullyPlannedTrips)
            {
                trips = trips.Where(s => s.Status == "CREATED" || s.Status == "FINISHED" || s.Status == "PLANNED");
            }
            else if (filter.FinishedTrips && filter.TripsConfirmed && filter.FullyPlannedTrips)
            {
                trips = trips.Where(s => s.Status == "FINISHED" || s.Status == "CONFIRMED" || s.Status == "PLANNED");
            }
            //Two parameters
            else if (filter.TripsAwaitingConfirmation && filter.TripsConfirmed)
            {
                trips = trips.Where(s => s.Status == "CREATED" || s.Status == "CONFIRMED");
            }
            else if (filter.TripsAwaitingConfirmation && filter.FullyPlannedTrips)
            {
                trips = trips.Where(s => s.Status == "CREATED" || s.Status == "PLANNED");
            }
            else if (filter.TripsAwaitingConfirmation && filter.FinishedTrips)
            {
                trips = trips.Where(s => s.Status == "CREATED" || s.Status == "FINISHED");
            }
            else if (filter.FullyPlannedTrips && filter.TripsConfirmed)
            {
                trips = trips.Where(s => s.Status == "PLANNED" || s.Status == "CONFIRMED");
            }
            else if (filter.FullyPlannedTrips && filter.TripsConfirmed)
            {
                trips = trips.Where(s => s.Status == "FINISHED" || s.Status == "CONFIRMED");
            }
            else if (filter.FullyPlannedTrips && filter.FinishedTrips)
            {
                trips = trips.Where(s => s.Status == "PLANNED" || s.Status == "FINISHED");
            }
            //ONE PARAMETER
            else if (filter.TripsAwaitingConfirmation)
            {
                trips = trips.Where(s => s.Status == "CREATED");
            }
            else if (filter.TripsConfirmed)
            {
                trips = trips.Where(s => s.Status == "CONFIRMED");
            }
            else if (filter.FullyPlannedTrips)
            {
                trips = trips.Where(s => s.Status == "PLANNED");
            }
            else if (filter.FinishedTrips)
            {
                trips = trips.Where(s => s.Status == "FINISHED");
            }
            else
            {
                return NotFound();
            }

            return Ok(trips.ToTripFilter().OrderByDescending(x => x.DepartureDate));
        }
    }
}
