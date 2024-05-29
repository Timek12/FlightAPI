using FlightAPI.Models;
using FlightAPI.Models.DTOs;

namespace FlightAPI.Repositories.Interfaces
{
    public interface IPlaneDapperRepository
    {
        Task<IEnumerable<PlaneDTO>> GetAll();
        Task<Plane?> GetPlaneById(int id);
        Task<PlaneDTO?> GetPlaneDTOById(int id);
    }
}
