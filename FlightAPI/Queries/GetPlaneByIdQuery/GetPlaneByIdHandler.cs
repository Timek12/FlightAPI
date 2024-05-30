using FlightAPI.Models.DTOs;
using FlightAPI.Services.Interfaces;
using MediatR;

namespace FlightAPI.Queries.GetPlaneByIdQuery
{
    public class GetPlaneByIdHandler : IRequestHandler<GetPlaneByIdQuery, PlaneDTO>
    {
        private readonly IPlaneQueryService _planeQueryService;

        public GetPlaneByIdHandler(IPlaneQueryService planeQueryService)
        {
            _planeQueryService = planeQueryService;
        }

        public async Task<PlaneDTO> Handle(GetPlaneByIdQuery request, CancellationToken cancellation)
        {
            return await _planeQueryService.GetPlaneById(request.Id);
        }

    }
}
