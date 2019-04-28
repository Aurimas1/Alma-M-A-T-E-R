using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repositories
{
    public class EmployeeRepository : IRepository<Employee>
    {
        private readonly ApiDbContext context;

        public EmployeeRepository(ApiDbContext context)
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

        public Employee Get(Func<Employee, bool> predicate)
        {
            return context.Employees.FirstOrDefault(predicate);
        }

        public IEnumerable<Employee> GetAll()
        {
            return context.Employees.ToList();
        }

        public IEnumerable<Employee> GetAll(Func<Employee, bool> predicate)
        {
            return context.Employees.Where(predicate);
        }

        public Employee Update(Employee item)
        {
            context.Employees.Update(item);
            return context.SaveChanges() == 1 ? item : null;
        }
    }
}
