using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repositories
{
    public class EmployeeRepository : IRepository<Employee>
    {
        private readonly IdentityDbContext context;

        public EmployeeRepository(IdentityDbContext context)
        {
            this.context = context;
        }

        public async Task<Employee> Add(Employee item)
        {
            await context.Employees.AddAsync(item);
            return context.SaveChanges() == 1 ? item : null;
        }

        public bool Delete(int id)
        {
            context.Employees.Remove(Get(id));
            return context.SaveChanges() == 1;
        }

        public Employee Get(int id)
        {
            return context.Employees.FirstOrDefault(x => x.EmployeeID == id);
        }

        public IEnumerable<Employee> GetAll()
        {
            return context.Employees.ToList();
        }

        public Employee Update(Employee item)
        {
            context.Employees.Update(item);
            return context.SaveChanges() == 1 ? item : null;
        }
    }
}
