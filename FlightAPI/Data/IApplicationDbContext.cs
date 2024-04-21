using FlightAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightAPI.Data
{
    public interface IApplicationDbContext
    {
        DbSet<ApplicationUser> ApplicationUsers { get; set; }
        DbSet<Flight> Flights { get; set; }
        DbSet<Plane> Planes { get; set; }
    
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
