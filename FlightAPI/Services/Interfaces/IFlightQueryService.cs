using FlightAPI.Models.DTOs;

namespace FlightAPI.Services.Interfaces
{
    public interface IFlightQueryService
    {
        Task<IEnumerable<FlightDTO>> GetAllFlights();
        Task<FlightDTO> GetFlightDTOById(int id);
    }
}
