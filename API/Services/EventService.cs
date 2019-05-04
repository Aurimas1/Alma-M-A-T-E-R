using System;
using System.Collections.Generic;
using System.Linq;
using API.Repositories;

namespace API.Services
{
    public class EventService : IEventService
    {
        private readonly IRepository<Event> repository;
        private readonly IRepository<Employee> employeeRepo;

        public EventService(IRepository<Event> repository, IRepository<Employee> employeeRepo)
        {
            this.repository = repository;
            this.employeeRepo = employeeRepo;
        }
        
        public IEnumerable<EventsInformationDTO> GetByDatesAndEmployees(DateTime from, DateTime to, IEnumerable<int> employeeIds)
        {

            //Get the list of events
            List<Event> events = repository.GetAll(e =>
               e.DateFrom >= from && e.DateFrom <= to && employeeIds.Contains(e.EmployeeID)).ToList();

            //prepare new DTO
            List<EventsInformationDTO> eventDTOs = new List<EventsInformationDTO>();
            foreach (Event e in events)
            {
                //Get the employee
                e.Employee = employeeRepo.Get(e.EmployeeID);
                
                //if an event takes more than 1 day, create start date/end date and in between dates eventDetails instances.
                if (!e.DateFrom.Date.Equals(e.DateTo.Date))
                {
                    CreateEventDetails(eventDTOs, e.DateFrom, e.DateFrom.ToString("HH:mm"), "24:00",
                        e.Employee.Name);
                    CreateEventDetails(eventDTOs, e.DateTo, "00:00", e.DateTo.ToString("HH:mm"),
                        e.Employee.Name);
                    
                    e.DateFrom = e.DateFrom.AddDays(1);
                    while (!e.DateFrom.Date.Equals(e.DateTo.Date))
                    {
                        CreateEventDetails(eventDTOs, e.DateFrom, "00:00", "24:00",
                            e.Employee.Name);
                        e.DateFrom = e.DateFrom.AddDays(1);
                    }
                }
                else
                {
                    CreateEventDetails(eventDTOs, e.DateFrom, e.DateFrom.ToString("HH:mm"), e.DateTo.ToString("HH:mm"),
                        e.Employee.Name);
                }
            }
            
            //sort
            eventDTOs = eventDTOs.OrderBy(e => e.Date).ToList();
            eventDTOs.ForEach(e => e.Events = e.Events.OrderBy(ev => ev.TimeFrom).ToList());


            return eventDTOs;
        }

        private void CreateEventDetails(List<EventsInformationDTO> DTOList, DateTime date,string timeFrom, string timeTo, string employeeName)
        {
            //Create new EventDetails
            var details = new EventsInformationDTO.EventDetails
            {
                TimeFrom = timeFrom,
                TimeTo = timeTo,
                FullName = employeeName
            };
            
            //Check if eventsDTO for this date already exists
            EventsInformationDTO eventDTO = DTOList.FirstOrDefault(e => e.Date == date.ToString("yyyy-MM-dd"));
            
            //If exists, append the list
            if (eventDTO != null)
                eventDTO.Events.Add(details);
            else
                DTOList.Add(new EventsInformationDTO
                {
                    Date = date.ToString("yyyy-MM-dd"),
                    Events = new List<EventsInformationDTO.EventDetails> {details}
                });
            
        }
    }

    public class EventsInformationDTO
    {
        public string Date;
        public class EventDetails
        {
            public string TimeFrom;
            public string TimeTo;
            public string FullName;
        }

        public List<EventDetails> Events;
    }
}