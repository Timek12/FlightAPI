using FlightAPI.Services.Interfaces;
using MediatR;

namespace FlightAPI.Commands.DeletePlaneCommand
{
    public class DeletePlaneHandler(IPlaneCommandService planeCommandService) : IRequestHandler<DeletePlaneCommand, Unit>
    {
        private readonly IPlaneCommandService _planeCommandService = planeCommandService;

        public async Task<Unit> Handle(DeletePlaneCommand command, CancellationToken cancellation)
        {
            await _planeCommandService.DeletePlane(command.Id);

            return Unit.Value;
        }
    }
}
