using FlightAPI.Models.DTOs;
using FlightAPI.Services.Interfaces;
using MediatR;

namespace FlightAPI.Queries.GetAllFlightsQuery
{
    public class GetAllFlightsHandler(IFlightQueryService flightQueryService) : IRequestHandler<GetAllFlightsQuery, IEnumerable<FlightDTO>>
    {
        private readonly IFlightQueryService _flightQueryService = flightQueryService;

        public async Task<IEnumerable<FlightDTO>> Handle(GetAllFlightsQuery request, CancellationToken cancellation)
        {
            return await _flightQueryService.GetAllFlights();
        }
    }
}
