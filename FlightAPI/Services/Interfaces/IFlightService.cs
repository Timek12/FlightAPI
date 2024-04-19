using FlightAPI.Models.DTOs;

namespace FlightAPI.Services.Interfaces
{
    public interface IFlightService
    {
        Task<IEnumerable<FlightDTO>> GetAllFlights();
        Task<FlightDTO> GetFlightDTOById(int id);
        Task<FlightDTO> UpdateFlight(int id, UpdateFlightDTO flightDTO);
        Task<FlightDTO> CreateFlight(CreateFlightDTO flightDTO);
        Task DeleteFlight(int id);
    }
}
