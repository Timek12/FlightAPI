using FlightAPI.Models;
using FlightAPI.Models.DTOs;
using MediatR;

namespace FlightAPI.Commands.UpdatePlaneCommand
{
    public class UpdatePlaneCommand(UpdatePlaneDTO updatePlaneDTO) : IRequest<PlaneDTO>
    {
        public UpdatePlaneDTO UpdatePlaneDTO { get; } = updatePlaneDTO;
    }
}
