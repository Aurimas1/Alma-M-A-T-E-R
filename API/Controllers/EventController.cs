using System;
using System.Collections.Generic;
using System.Globalization;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Organiser")]
    public class EventController : ControllerBase
    {
        private readonly IEventService service;

        public EventController(IEventService service)
        {
            this.service = service;
        }

        [HttpGet]
        public IEnumerable<EventsInformationDTO> Get(string dateFrom, string dateTo, [FromQuery] int[] employeeIds)
        {
            DateTime from = DateTime.ParseExact(dateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime to = DateTime.ParseExact(dateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            to = to.AddDays(1);
            var data = service.GetByDatesAndEmployees(from, to, employeeIds);
            return data;
        }
        
    }
}