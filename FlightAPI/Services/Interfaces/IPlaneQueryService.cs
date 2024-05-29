using FlightAPI.Models.DTOs;

namespace FlightAPI.Services.Interfaces
{
    public interface IPlaneQueryService
    {
        Task<IEnumerable<PlaneDTO>> GetAllPlanes();
        Task<PlaneDTO> GetPlaneById(int id);
    }
}
