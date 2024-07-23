using FlightAPI.Models.DTOs;
using FlightAPI.Services.Interfaces;
using MediatR;

namespace FlightAPI.Commands.UpdatePlaneCommand
{
    public class UpdatePlaneHandler(IPlaneCommandService planeCommandService) : IRequestHandler<UpdatePlaneCommand, PlaneDTO>
    {
        private readonly IPlaneCommandService _planeCommandService = planeCommandService;

        public async Task<PlaneDTO> Handle(UpdatePlaneCommand command, CancellationToken cancellation)
        {
            return await _planeCommandService.UpdatePlane(command.UpdatePlaneDTO);
        }
    }
}
