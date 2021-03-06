﻿using API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class EmployeeToTripService : IEmployeeToTripService
    {
        private readonly IRepository<EmployeeToTrip> repository;

        public EmployeeToTripService(IRepository<EmployeeToTrip> repository)
        {
            this.repository = repository;
        }

        public Task<EmployeeToTrip> Add(EmployeeToTrip item)
        {
            return repository.Add(item);
        }

        public bool Remove(int id)
        {
            return repository.Delete(id);
        }

        public EmployeeToTrip Update(EmployeeToTrip item)
        {
            return repository.Update(item);
        }

        public EmployeeToTrip GetByID(int id)
        {
            return repository.Get(id);
        }
    }
}
