using FlightAPI.Models.DTOs;
using FlightAPI.Services.Interfaces;
using MediatR;

namespace FlightAPI.Queries.GetAllPlanesQuery
{
    public class GetAllPlanesHandler(IPlaneQueryService planeQueryService) : IRequestHandler<GetAllPlanesQuery, IEnumerable<PlaneDTO>>
    {
        private readonly IPlaneQueryService _planeQueryService = planeQueryService;

        public async Task<IEnumerable<PlaneDTO>> Handle(GetAllPlanesQuery request, CancellationToken cancellationToken)
        {
            return await _planeQueryService.GetAllPlanes();
        }
    }
}
