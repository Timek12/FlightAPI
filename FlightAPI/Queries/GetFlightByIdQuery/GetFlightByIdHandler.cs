using FlightAPI.Models.DTOs;
using FlightAPI.Services.Interfaces;
using MediatR;

namespace FlightAPI.Queries.GetFlightByIdQuery
{
    public class GetFlightByIdHandler : IRequestHandler<GetFlightByIdQuery, FlightDTO>
    {
        private readonly IFlightQueryService _flightQueryService;

        public GetFlightByIdHandler(IFlightQueryService flightQueryService)
        {
            _flightQueryService = flightQueryService;
        }

        public async Task<FlightDTO> Handle(GetFlightByIdQuery request, CancellationToken cancellationToken)
        {
            return await _flightQueryService.GetFlightDTOById(request.Id);
        }
    }
}
