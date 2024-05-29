using FlightAPI.Models.DTOs;
using FlightAPI.Services.Interfaces;
using MediatR;

namespace FlightAPI.Commands.CreatePlaneCommand
{
    public class CreatePlaneHandler : IRequestHandler<CreatePlaneCommand, PlaneDTO>
    {
        private readonly IPlaneCommandService _planeCommandService;

        public CreatePlaneHandler(IPlaneCommandService planeCommandService)
        {
            _planeCommandService = planeCommandService;
        }

        public async Task<PlaneDTO> Handle(CreatePlaneCommand command, CancellationToken cancellation)
        {
            return await _planeCommandService.CreatePlane(command.CreatePlaneDTO);
        }
    }
}
