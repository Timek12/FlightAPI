using AutoMapper;
using FlightAPI.Models.DTOs;
using FlightAPI.Repositories.Interfaces;
using FlightAPI.Services.Interfaces;

namespace FlightAPI.Services.Implementations
{
    public class FlightService(IFlightRepository flightRepository, IPlaneRepository planeRepository, IMapper mapper) : IFlightService
    {
        private readonly IFlightRepository _flightRepository = flightRepository;
        private readonly IPlaneRepository _planeRepository = planeRepository;

        public async Task<FlightDTO> CreateFlight(CreateFlightDTO flightDTO)
        {
            if (flightDTO is null)
            {
                throw new ArgumentNullException(nameof(flightDTO), "Flight data cannot be null");
            }

            if (flightDTO.PlaneId <= 0)
            {
                throw new ArgumentException("PlaneId must be greater than 0");
            }

            var plane = await _planeRepository.GetPlaneById(flightDTO.PlaneId);
            if (plane is null)
            {
                throw new KeyNotFoundException("Plane with given id does not exist");
            }

            return await _flightRepository.Create(flightDTO);
        }

        public async Task Delete(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("FlightId must be greater than 0");
            }

            var flightFromDb = await _flightRepository.GetFlightById(id);

            if (flightFromDb is null)
            {
                throw new KeyNotFoundException("Flight with given id does not exist");
            }

            await _flightRepository.Delete(flightFromDb);
        }

        public async Task<IEnumerable<FlightDTO>> GetAllFlights()
        {
            return await _flightRepository.GetAll();
        }

        public async Task<FlightDTO> GetFlightDTOById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("FlightId must be greater than 0");
            }

            FlightDTO? flightDTO = await _flightRepository.GetFlightDTOById(id);

            if (flightDTO is null)
            {
                throw new KeyNotFoundException("Flight with given id does not exist");
            }

            return flightDTO;
        }

        public async Task<FlightDTO> UpdateFlight(int id, UpdateFlightDTO flightDTO)
        {
            if (flightDTO is null)
            {
                throw new ArgumentNullException(nameof(flightDTO), "Flight data cannot be null");
            }

            if (flightDTO.Id <= 0)
            {
                throw new ArgumentException("FlightId must be greater than 0");
            }

            if (flightDTO.PlaneId <= 0)
            {
                throw new ArgumentException("PlaneId must be greater than 0");
            }

            if (id != flightDTO.Id)
            {
                throw new ArgumentException("Mismatch between URL id and flightDTO id");
            }

            var flightFromDb = await _flightRepository.GetFlightById(flightDTO.Id);

            if (flightFromDb is null)
            {
                throw new KeyNotFoundException("Flight with given id does not exist");
            }

            return await _flightRepository.Update(flightDTO, flightFromDb);
        }

    }
}
