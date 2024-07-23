using FlightAPI.Models.DTOs;
using FlightAPI.Services.Interfaces;
using MediatR;

namespace FlightAPI.Commands.CreatePlaneCommand
{
    public class CreatePlaneHandler(IPlaneCommandService planeCommandService) : IRequestHandler<CreatePlaneCommand, PlaneDTO>
    {
        private readonly IPlaneCommandService _planeCommandService = planeCommandService;

        public async Task<PlaneDTO> Handle(CreatePlaneCommand command, CancellationToken cancellation)
        {
            return await _planeCommandService.CreatePlane(command.CreatePlaneDTO);
        }
    }
}
