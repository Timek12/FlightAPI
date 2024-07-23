using FlightAPI.Models.DTOs;
using MediatR;

namespace FlightAPI.Commands.CreatePlaneCommand
{
    public class CreatePlaneCommand(CreatePlaneDTO createPlaneDTO) : IRequest<PlaneDTO>
    {
        public CreatePlaneDTO CreatePlaneDTO { get; } = createPlaneDTO;
    }
}
