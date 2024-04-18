using FlightAPI.Models;
using FlightAPI.Models.DTOs;

namespace FlightAPI.Repositories.Interfaces
{
    public interface IFlightRepository
    {
        Task<IEnumerable<FlightDTO>> GetAll();
        Task<FlightDTO?> GetFlightDTOById(int id);
        Task<Flight?> GetFlightById(int id);
        Task<FlightDTO> Update(UpdateFlightDTO flightDTO, Flight flight);
        Task<FlightDTO> Create(CreateFlightDTO flightDTO);
        Task Delete(Flight flight);
    }
}
