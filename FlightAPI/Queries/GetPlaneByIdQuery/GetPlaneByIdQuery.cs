using FlightAPI.Models.DTOs;
using MediatR;

namespace FlightAPI.Queries.GetPlaneByIdQuery
{
    public class GetPlaneByIdQuery(int id) : IRequest<PlaneDTO>
    {
        public int Id { get; } = id;
    }
}
