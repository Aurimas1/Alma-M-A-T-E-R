using API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public interface IEmployeeService
    {
        IEnumerable<Employee> GetAll();
    }
}
