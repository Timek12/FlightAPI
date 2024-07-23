using FlightAPI.Data;
using FlightAPI.Exceptions;
using FlightAPI.Models.DTOs;
using FlightAPI.Repositories.Interfaces;
using FlightAPI.Services.Interfaces;

namespace FlightAPI.Services.Implementations
{
    public class PlaneCommandService(IPlaneRepository planeRepository, IPlaneDapperRepository planeDapperRepository, ILogger<PlaneCommandService> logger) : IPlaneCommandService
    {
        private readonly IPlaneRepository _planeRepository = planeRepository;
        private readonly IPlaneDapperRepository _planeDapperRepository = planeDapperRepository;
        private readonly ILogger<PlaneCommandService> _logger = logger;

        public async Task<PlaneDTO> CreatePlane(CreatePlaneDTO createPlaneDTO)
        {
            _logger.LogInformation($"Creating a new plane model: {createPlaneDTO.Model}");

            if(createPlaneDTO is null)
            {
                throw new NullPlaneDataException();
            }


            return await _planeRepository.CreatePlane(createPlaneDTO);
        }

        public async Task DeletePlane(int id)
        {
            _logger.LogInformation($"Deleting plane");

            if (id < 0)
            {
                throw new InvalidPlaneIdException();
            }

            var planeFromDb = await _planeDapperRepository.GetPlaneById(id);
            if(planeFromDb is null)
            {
                throw new PlaneNotFoundException();
            }

            await _planeRepository.DeletePlane(planeFromDb);
        }

        public async Task<PlaneDTO> UpdatePlane(UpdatePlaneDTO updatePlaneDTO)
        {
            _logger.LogInformation($"Updating plane with id: {updatePlaneDTO.Id}");

            if(updatePlaneDTO is null)
            {
                throw new NullPlaneDataException();
            }

            if (updatePlaneDTO.Id < 0)
            {
                throw new InvalidPlaneIdException();
            }

            var planeFromDb = await _planeDapperRepository.GetPlaneById(updatePlaneDTO.Id);
            if (planeFromDb is null)
            {
                throw new PlaneNotFoundException();
            }

            return await _planeRepository.UpdatePlane(updatePlaneDTO, planeFromDb);
        }
    }
}
