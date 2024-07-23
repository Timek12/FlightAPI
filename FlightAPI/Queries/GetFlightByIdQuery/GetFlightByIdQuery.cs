using FlightAPI.Models.DTOs;
using MediatR;

namespace FlightAPI.Queries.GetFlightByIdQuery
{
    public class GetFlightByIdQuery(int id) : IRequest<FlightDTO>
    {
        public int Id { get; } = id;
    }
}
