using FlightAPI.Models.DTOs;
using MediatR;

namespace FlightAPI.Queries.GetAllPlanesQuery
{
    public class GetAllPlanesQuery : IRequest<IEnumerable<PlaneDTO>>
    {
        
    }
}
