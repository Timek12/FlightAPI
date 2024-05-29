using FlightAPI.Models;
using FlightAPI.Models.DTOs;
using MediatR;

namespace FlightAPI.Commands.UpdatePlaneCommand
{
    public class UpdatePlaneCommand : IRequest<PlaneDTO>
    {
        public UpdatePlaneDTO UpdatePlaneDTO { get; }

        public UpdatePlaneCommand(UpdatePlaneDTO updatePlaneDTO)
        {
            UpdatePlaneDTO = updatePlaneDTO;
        }
    }
}
