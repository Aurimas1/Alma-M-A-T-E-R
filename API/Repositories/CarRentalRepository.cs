using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repositories
{
    public class CarRentalRepository : IRepository<CarRental>
    {
        private readonly ApiDbContext context;

        public CarRentalRepository(ApiDbContext context)
        {
            this.context = context;
        }
        
        public async Task<CarRental> Add(CarRental item)
        {
            await context.CarRentals.AddAsync(item);
            return context.SaveChanges() == 1 ? item : null;
        }

        public IEnumerable<CarRental> GetAll()
        {
            return context.CarRentals.ToList();
        }

        public IEnumerable<CarRental> GetAll(Func<CarRental, bool> predicate)
        {
            return context.CarRentals.ToList().Where(predicate);
 
        }

        public CarRental Get(int id)
        {
            return context.CarRentals.FirstOrDefault(x => x.CarRentalID == id);
        }

        public CarRental Get(Func<CarRental, bool> predicate)
        {
            return context.CarRentals.FirstOrDefault(predicate);
        }

        public CarRental Update(CarRental item)
        {
            context.CarRentals.Update(item);
            return context.SaveChanges() == 1 ? item : null;
        }

        public bool Delete(int id)
        {
            context.CarRentals.Remove(Get(id));
            return context.SaveChanges() == 1;
        }
    }
}