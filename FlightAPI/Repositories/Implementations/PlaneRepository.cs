using FlightAPI.Data;
using FlightAPI.Models;
using FlightAPI.Repositories.Interfaces;

namespace FlightAPI.Repositories.Implementations
{
    public class PlaneRepository(ApplicationDbContext db) : IPlaneRepository
    {
        readonly ApplicationDbContext _db = db;

        public async Task<Plane?> GetPlaneById(int id)
        {
            return await _db.Planes.FindAsync(id);
        }
    }
}
