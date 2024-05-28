using FlightAPI.Exceptions;
using FlightAPI.Middleware;
using FlightAPI.Models.DTOs;
using FlightAPI.Repositories.Interfaces;
using FlightAPI.Services.Interfaces;

namespace FlightAPI.Services.Implementations
{
    public class FlightCommandService(IFlightRepository flightRepository, IFlightDapperRepository flightDapperRepository,
        IPlaneRepository planeRepository,
        ILogger<ExceptionHandlingMiddleware> logger) : IFlightCommandService
    {
        private readonly IFlightRepository _flightRepository = flightRepository;
        private readonly IFlightDapperRepository _flightDapperRepository = flightDapperRepository;  
        private readonly IPlaneRepository _planeRepository = planeRepository;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;

        public async Task<FlightDTO> CreateFlight(CreateFlightDTO flightDTO)
        {
            if (flightDTO is null)
            {
                throw new NullFlightDataException();
            }

            _logger.LogInformation($"Creating a new flight with Flight Number: {flightDTO.FlightNumber}.");


            if (flightDTO.PlaneId <= 0)
            {
                throw new InvalidPlaneIdException();
            }

            var plane = await _planeRepository.GetPlaneById(flightDTO.PlaneId);

            if (plane is null)
            {
                throw new PlaneNotFoundException();
            }

            return await _flightRepository.Create(flightDTO);
        }

        public async Task DeleteFlight(int id)
        {
            _logger.LogInformation($"Deleting flight with ID: {id}.");
            if (id <= 0)
            {
                throw new InvalidFlightIdException();
            }

            var flightFromDb = await _flightDapperRepository.GetFlightById(id);

            if (flightFromDb is null)
            {
                throw new FlightNotFoundException();
            }

            await _flightRepository.Delete(flightFromDb);
        }

        public async Task<FlightDTO> UpdateFlight(int id, UpdateFlightDTO flightDTO)
        {
            if (flightDTO is null)
            {
                throw new NullFlightDataException();
            }

            _logger.LogInformation($"Updating flight with ID: {id}.");


            if (flightDTO.Id <= 0)
            {
                throw new InvalidFlightIdException();
            }

            if (flightDTO.PlaneId <= 0)
            {
                throw new InvalidPlaneIdException();
            }

            if (id != flightDTO.Id)
            {
                throw new InvalidFlightDataException();
            }

            var flightFromDb = await _flightDapperRepository.GetFlightById(flightDTO.Id);

            if (flightFromDb is null)
            {
                throw new FlightNotFoundException();
            }

            return await _flightRepository.Update(flightDTO, flightFromDb);
        }

    }
}
