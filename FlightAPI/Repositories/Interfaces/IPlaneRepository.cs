using FlightAPI.Models;

namespace FlightAPI.Repositories.Interfaces
{
    public interface IPlaneRepository
    {
        Task<Plane?> GetPlaneById(int id);
    }
}
