using FlightAPI.Models;
using FlightAPI.Models.DTOs;

namespace FlightAPI.Repositories.Interfaces
{
    public interface IPlaneRepository
    {
        Task<PlaneDTO> CreatePlane(CreatePlaneDTO createPlaneDTO);
        Task<PlaneDTO> UpdatePlane(UpdatePlaneDTO updatePlaneDTO, Plane plane);
        void DeletePlane(Plane plane);
    }
}
