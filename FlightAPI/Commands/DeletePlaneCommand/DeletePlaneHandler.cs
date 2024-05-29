using FlightAPI.Services.Interfaces;
using MediatR;

namespace FlightAPI.Commands.DeletePlaneCommand
{
    public class DeletePlaneHandler : IRequestHandler<DeletePlaneCommand, Unit>
    {
        private readonly IPlaneCommandService _planeCommandService;

        public DeletePlaneHandler(IPlaneCommandService planeCommandService)
        {
            _planeCommandService = planeCommandService;
        }

        public async Task<Unit> Handle(DeletePlaneCommand command, CancellationToken cancellation)
        {
            await _planeCommandService.DeletePlane(command.Id);

            return Unit.Value;
        }
    }
}
