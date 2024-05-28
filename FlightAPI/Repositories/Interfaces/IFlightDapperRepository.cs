using FlightAPI.Data;
using FlightAPI.Models.DTOs;
using FlightAPI.Models;

namespace FlightAPI.Repositories.Interfaces
{
    public interface IFlightDapperRepository
    {
        Task<IEnumerable<FlightDTO>> GetAll();
        Task<FlightDTO?> GetFlightDTOById(int id);
        Task<Flight?> GetFlightById(int id);
    }
}
