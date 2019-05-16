using System.Collections.Generic;
using System.Threading.Tasks;
using API.Controllers;

namespace API.Services
{
    public interface IEmployeeService
    {
        IEnumerable<Employee> GetAll();
        Task<Employee> Ensure(string email, string name);
        void UpdateEmployees(IEnumerable<EmployeeRolesDTO> employees);
    }
}
