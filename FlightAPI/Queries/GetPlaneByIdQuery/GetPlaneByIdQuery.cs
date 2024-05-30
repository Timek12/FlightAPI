using FlightAPI.Models.DTOs;
using MediatR;

namespace FlightAPI.Queries.GetPlaneByIdQuery
{
    public class GetPlaneByIdQuery : IRequest<PlaneDTO>
    {
        public int Id { get; }

        public GetPlaneByIdQuery(int id)
        {
            Id = id;
        }
    }
}
