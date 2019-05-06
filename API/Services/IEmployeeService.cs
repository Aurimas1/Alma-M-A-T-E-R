using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services
{
    public interface IEmployeeService
    {
        IEnumerable<Employee> GetAll();
        Task<Employee> Ensure(string email, string name);
    }
}
