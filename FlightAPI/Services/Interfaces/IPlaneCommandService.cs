using FlightAPI.Models.DTOs;

namespace FlightAPI.Services.Interfaces
{
    public interface IPlaneCommandService
    {
        Task<PlaneDTO> CreatePlane(CreatePlaneDTO createPlaneDTO);
        Task<PlaneDTO> UpdatePlane(UpdatePlaneDTO updatePlaneDTO);
        Task DeletePlane(int id);
    }
}
