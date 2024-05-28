using FlightAPI.Models.DTOs;

namespace FlightAPI.Services.Interfaces
{
    public interface IFlightCommandService
    {
        Task<FlightDTO> UpdateFlight(int id, UpdateFlightDTO flightDTO);
        Task<FlightDTO> CreateFlight(CreateFlightDTO flightDTO);
        Task DeleteFlight(int id);
    }
}
