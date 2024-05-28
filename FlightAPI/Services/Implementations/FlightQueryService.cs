using FlightAPI.Exceptions;
using FlightAPI.Middleware;
using FlightAPI.Models.DTOs;
using FlightAPI.Repositories.Interfaces;
using FlightAPI.Services.Interfaces;

namespace FlightAPI.Services.Implementations
{
    public class FlightQueryService(IFlightDapperRepository flightDapperRepository, ILogger<ExceptionHandlingMiddleware> logger) : IFlightQueryService
    {
        private readonly IFlightDapperRepository _flightDapperRepository = flightDapperRepository;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;

        public async Task<IEnumerable<FlightDTO>> GetAllFlights()
        {
            _logger.LogInformation("Getting all flights.");
            return await _flightDapperRepository.GetAll();
        }

        public async Task<FlightDTO> GetFlightDTOById(int id)
        {
            _logger.LogInformation($"Getting flight with ID: {id}.");
            if (id <= 0)
            {
                throw new InvalidFlightIdException();
            }

            FlightDTO? flightDTO = await _flightDapperRepository.GetFlightDTOById(id);

            if (flightDTO is null)
            {
                throw new FlightNotFoundException();
            }

            return flightDTO;
        }
    }
}
