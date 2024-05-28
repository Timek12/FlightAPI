using FlightAPI.Models;
using FlightAPI.Models.DTOs;

namespace FlightAPI.Repositories.Interfaces
{
    public interface IFlightRepository
    {
        Task<FlightDTO> Update(UpdateFlightDTO flightDTO, Flight flight);
        Task<FlightDTO> Create(CreateFlightDTO flightDTO);
        Task Delete(Flight flight);
    }
}
