using FlightAPI.Models.DTOs;
using MediatR;

namespace FlightAPI.Queries.GetFlightByIdQuery
{
    public class GetFlightByIdQuery : IRequest<FlightDTO>
    {
        public int Id { get; }
    
        public GetFlightByIdQuery(int id)
        {
            Id = id;
        }
    }
}
