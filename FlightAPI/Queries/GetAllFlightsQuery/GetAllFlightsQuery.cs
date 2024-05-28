using FlightAPI.Models.DTOs;
using MediatR;

namespace FlightAPI.Queries.GetAllFlightsQuery
{
    public class GetAllFlightsQuery : IRequest<IEnumerable<FlightDTO>>
    {

    }
}
