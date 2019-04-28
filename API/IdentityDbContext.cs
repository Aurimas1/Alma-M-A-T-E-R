using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public class IdentityDbContext : IdentityDbContext<User, Role, string>
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Apartment> Apartments { get; set; }
        public DbSet<CarRental> CarRentals { get; set; }
        public DbSet<EmployeeToTrip> EmployeeToTrips { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<GasCompensation> GasCompensations { get; set; }
        public DbSet<Office> Offices { get; set; }
        public DbSet<PlaneTicket> PlaneTickets { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Trip> Trips { get; set; }
    }
}
