using AutoMapper;
using FlightAPI.Exceptions;
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
                throw new NullFlightDataException();
            }

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
            if (id <= 0)
            {
                throw new InvalidFlightIdException();
            }

            var flightFromDb = await _flightRepository.GetFlightById(id);

            if (flightFromDb is null)
            {
                throw new FlightNotFoundException();
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
                throw new InvalidFlightIdException();
            }

            FlightDTO? flightDTO = await _flightRepository.GetFlightDTOById(id);

            if (flightDTO is null)
            {
                throw new FlightNotFoundException();
            }

            return flightDTO;
        }

        public async Task<FlightDTO> UpdateFlight(int id, UpdateFlightDTO flightDTO)
        {
            if (flightDTO is null)
            {
                throw new NullFlightDataException();
            }

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

            var flightFromDb = await _flightRepository.GetFlightById(flightDTO.Id);

            if (flightFromDb is null)
            {
                throw new FlightNotFoundException();
            }

            return await _flightRepository.Update(flightDTO, flightFromDb);
        }

    }
}
