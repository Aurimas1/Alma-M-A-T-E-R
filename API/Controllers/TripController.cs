using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        private readonly IOfficeService officeService;
        private readonly IApartmentService apartmentService;

        public TripController(ITripService service, IEmployeeToTripService employeeToTripService, IOfficeService officeService, IApartmentService apartmentService)
        {
            this.service = service;
            this.employeeToTripService = employeeToTripService;
            this.officeService = officeService;
            this.apartmentService = apartmentService;
        }

        // POST api/Trip
        [HttpPost]
        public async Task<OkResult> Add([FromBody]CreateTrip item)
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
        public async Task<OkResult> Update([FromBody]UpdateTrip item, int id)
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

            foreach (var e in trip.EmployeesToTrip)
            {
                if (!item.Employees.Contains(e.EmployeeID))
                {
                    employeeToTripService.Remove(e.EmployeeToTripID);
                }
            }

            trip.Reservations = item.Rooms.Select(x => new Reservation
            {
                ApartmentID = trip.ArrivalOffice.Apartaments.First(a => a.RoomNumber == x.RoomID).ApartmentID,
                EmployeeID = x.EmployeeID,
                CheckIn = trip.DepartureDate,
                CheckOut = trip.ReturnDate,
                TripID = trip.TripID,
                ReservationUrl = "-",
            }).ToList();

            var result = service.Update(trip);

            return Ok();
        }

        // Post api/Trip/gasCompensation
        [Route("gasCompensation")]
        [HttpPost]
        public async Task<OkResult> AddGasCompensation([FromBody]GasCompensation item)
        {
            await service.SaveGasCompensation(new GasCompensation()
            {
                TripID = item.TripID,
                EmployeeID = item.EmployeeID,
                Price = item.Price,
            });

            return Ok();
        }

        // Post api/Trip/carRental
        [Route("carRental")]
        [HttpPost]
        public async Task<OkResult> AddCarRental([FromBody]CarRental item)
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
            });

            return Ok();
        }

        // Post api/Trip/planeTicket
        [Route("planeTicket")]
        [HttpPost]
        public async Task<OkResult> AddPlaneTicket([FromBody]PlaneTicket item)
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
            });

            return Ok();
        }

        // Post api/Trip/hotel
        [Route("hotel")]
        [HttpPost]
        public async Task<OkResult> AddHouse([FromBody]Hotel item)
        {
            var apartament = await service.SaveHotelorHome(new Apartment()
            {
                OfficeId = null,
                Address = item.Address,
                Name = item.Name,
                Price = item.Price,
                RoomNumber = item.RoomNumber,
                Type = "HOTEL",
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
        public async Task<OkResult> AddHome([FromBody]Home item)
        {
            var apartament = await service.SaveHotelorHome(new Apartment()
            {
                OfficeId = null,
                Address = item.Address,
                Name = null,
                Price = 0,
                RoomNumber = 0,
                Type = "HOME",
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
            var trips = service.GetAll();

            var fullTrips = trips.Select(t =>
            {

                double confirmedProcentage = 0;
                var employees = 0;
                var employeeCount = "0/0";

                double accomodationProcentage = 0;
                var accomodationCount = "0/0";

                double planeTicketProcentage = 0;
                var planeTicketCount = "0/0";

                double carRentalProcentage = 0;
                var carRentalCount = "0/0";

                employees = 0;
                foreach (var i in t.EmployeesToTrip)
                {
                    if (i.Status == "APPROVED")
                    {
                        employees++;
                    }
                }

                confirmedProcentage = ((double)employees / (double)t.EmployeesToTrip.Count);
                confirmedProcentage = Math.Round(confirmedProcentage, 1, MidpointRounding.AwayFromZero) * 100;
                employeeCount = employees + "/" + t.EmployeesToTrip.Count;

                accomodationProcentage = (double)(t.Reservations?.Count ?? 0) / (double)t.EmployeesToTrip.Count;
                accomodationProcentage = Math.Round(accomodationProcentage, 1, MidpointRounding.AwayFromZero) * 100;


                accomodationCount = (t.Reservations?.Count ?? 0) + "/" + t.EmployeesToTrip.Count;

                if (t.IsPlaneNeeded)
                {
                    planeTicketProcentage = (double)(t.PlaneTickets?.Count ?? 0) / (double)t.EmployeesToTrip.Count;
                    planeTicketProcentage = Math.Round(planeTicketProcentage, 1, MidpointRounding.AwayFromZero) * 100;
                    planeTicketCount = (t.PlaneTickets?.Count ?? 0) + "/" + t.EmployeesToTrip.Count;
                }
                else planeTicketProcentage = 100;

                if (t.IsCarRentalNeeded)
                {
                    carRentalProcentage = (double)(t.CarRentals?.Count ?? 0) / (double)t.EmployeesToTrip.Count;
                    carRentalProcentage = Math.Round(carRentalProcentage, 1, MidpointRounding.AwayFromZero) * 100;
                    carRentalCount = (t.CarRentals?.Count ?? 0) + "/" + t.EmployeesToTrip.Count;
                }
                else carRentalProcentage = 100;

                return new TripFilter
                {
                    ID = t.TripID,
                    ArrivalOffice = t.ArrivalOffice,
                    DepartureOffice = t.DepartureOffice,
                    ReturnDate = t.ReturnDate,
                    DepartureDate = t.DepartureDate,
                    ConfirmedProcentage = confirmedProcentage,
                    EmployeeCount = employeeCount,
                    AccomodationProcentage = accomodationProcentage,
                    AccomodationCount = accomodationCount,
                    PlaneTicketProcentage = planeTicketProcentage,
                    PlaneTicketCount = planeTicketCount,
                    CarRentalProcentage = carRentalProcentage,
                    CarRentalCount = carRentalCount,
                    Status = t.Status,
                    EmployeeEmailList = t.EmployeesToTrip.Select(x => x.Employee.Email),
                };
            });

            return fullTrips;
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

                IsPlaneNeeded = trip.IsPlaneNeeded,
                FlightCompany = trip.PlaneTickets?.Select(x => x.Airport),
                Airport = trip.PlaneTickets?.Select(x => x.FlightCompany),
                ForwardFlight = trip.PlaneTickets?.Select(x => x.ForwardFlightDate),
                ReturnFlight = trip.PlaneTickets?.Select(x => x.ReturnFlightDate),
                TicketFile = trip.PlaneTickets?.Select(x => x.PlaneTicketUrl),
                Accomodation = trip.Reservations?.Select(x => x.Apartment.Name),
                Address = trip.Reservations?.Select(x => x.Apartment.Address),
                RoomNumber = trip.Reservations?.Select(x => x.Apartment.RoomNumber),
                CheckIn = trip.Reservations?.Select(x => x.CheckIn),
                CheckOut = trip.Reservations?.Select(x => x.CheckOut),
                AccomodationUrl = trip.Reservations?.Select(x => x.ReservationUrl),

                IsCarRentalNeeded = trip.IsCarRentalNeeded,
                RentalCompany = trip.CarRentals?.Select(x => x.CarRentalCompany),
                CarPickupAddress = trip.CarRentals?.Select(x => x.CarPickupAddress),
                CarIssueDate = trip.CarRentals?.Select(x => x.CarIssueDate),
                CarReturnDate = trip.CarRentals?.Select(x => x.CarReturnDate),
                CarRentalUrl = trip.CarRentals?.Select(x => x.CarRentalUrl),

                IsCarCompensationNeeded = trip.IsCarCompensationNeeded,
                GasCompensation = trip.GasCompensations?.Select(x => x.Employee.Name),
                Amount = trip.GasCompensations?.Select(x => x.Price),
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
        public IEnumerable<TripFilter> Get(bool tripsAwaitingConfirmation, bool tripsConfirmed, bool fullyPlannedTrips, bool finishedTrips, bool myOrganizedTrips, bool otherOrganizedTrips, string dateFrom, string dateTo)
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

            var trips = Get();

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
                return trips = null;
            }

            var user = User.Claims.First(x => x.Type == ClaimTypes.Email).Value;


            if (filter.MyOrganizedTrips && filter.OtherOrganizedTrips)
            {
                trips = trips.Where(s => s.DepartureDate >= filter.DateFrom && s.ReturnDate <= filter.DateTo).OrderByDescending(x => x.DepartureDate);
            }
            else if (filter.MyOrganizedTrips)
            {
                trips = trips.Where(s => s.DepartureDate >= filter.DateFrom && s.ReturnDate <= filter.DateTo && s.EmployeeEmailList.Contains(user)).OrderByDescending(x => x.DepartureDate);
            }
            else if (filter.OtherOrganizedTrips)
            {
                trips = trips.Where(s => s.DepartureDate >= filter.DateFrom && s.ReturnDate <= filter.DateTo && !s.EmployeeEmailList.Contains(user)).OrderByDescending(x => x.DepartureDate);
            }
            else
            {
                trips = null;
            }

            return trips;
        }
    }
}
