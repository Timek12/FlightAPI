using FlightAPI.Models.DTOs;
using MediatR;

namespace FlightAPI.Commands.CreatePlaneCommand
{
    public class CreatePlaneCommand : IRequest<PlaneDTO>
    {
        public CreatePlaneDTO CreatePlaneDTO { get; }

        public CreatePlaneCommand(CreatePlaneDTO createPlaneDTO)
        {
            CreatePlaneDTO = createPlaneDTO;
        }
    }
}
