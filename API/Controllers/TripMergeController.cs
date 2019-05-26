using System;
using System.Collections.Generic;
using System.Linq;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    //[Authorize(Roles = "Organiser, Admin")]
    public class TripMergeController : ControllerBase
    {
        private readonly ITripMerge service;

        public TripMergeController(ITripMerge service)
        {
            this.service = service;
        }
        
        // GET
        [HttpGet]
        [Route("canMerge/{id}")]
        public Boolean CanMerge(int id)
        {
            return service.TripCanBeMerged(id);
        }
        
        // GET
        [HttpGet]
        [Route("{id}")]
        public List<TripMergeDTO> GetTripsForMerging(int id)
        {
            return service.GetTripsForMerging(id);
        }
        
        // GET
        [HttpGet]
        [Route("dates")]
        public object GetMergingTripsDates(int tripId1, int tripId2)
        {
            return service.GetTripDates(tripId1, tripId2);
        }
        
        // GET
        [HttpGet]
        [Route("employees")]
        public List<int> GetMergingTripsDEmployeesIds(int tripId1, int tripId2)
        {
            return service.GetTripEmployeesIds(tripId1, tripId2);
        }

        [HttpPut]
        public IActionResult MergeTrips([FromBody] MergeTripsData mergeTripsData)
        {
            if (service.MergeTrips(mergeTripsData) != -1) return Ok();
            return Conflict("Merging failed.");
        }

    }

    public class MergeTripsData
    {
        public int Trip1Id { get; set; }
        public int Trip2Id { get; set; }
        public DateTime departureDate { get; set; }
        public DateTime returnDate { get; set; }
    }
}