using FlightAPI.Exceptions;
using FlightAPI.Models.DTOs;
using FlightAPI.Repositories.Interfaces;
using FlightAPI.Services.Interfaces;

namespace FlightAPI.Services.Implementations
{
    public class PlaneQueryService(IPlaneDapperRepository planeDapperRepository, ILogger<PlaneQueryService> logger) : IPlaneQueryService
    {
        private readonly IPlaneDapperRepository _planeDapperRepository = planeDapperRepository;
        private readonly ILogger<PlaneQueryService> _logger = logger;

        public async Task<IEnumerable<PlaneDTO>> GetAllPlanes()
        {
            _logger.LogInformation("Getting all planes.");
            return await _planeDapperRepository.GetAll();
        }

        public async Task<PlaneDTO> GetPlaneById(int id)
        {
            _logger.LogInformation($"Getting plane with id: {id}");

            if(id < 0)
            {
                throw new InvalidPlaneIdException();
            }

            PlaneDTO? planeDTO = await _planeDapperRepository.GetPlaneDTOById(id);

            return planeDTO is null ? throw new PlaneNotFoundException() : planeDTO;
        }
    }
}
