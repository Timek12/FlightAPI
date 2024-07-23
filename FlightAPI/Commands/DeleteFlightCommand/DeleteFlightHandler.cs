using FlightAPI.Services.Interfaces;
using MediatR;

namespace FlightAPI.Commands.DeleteFlightCommand
{
    public class DeleteFlightHandler(IFlightCommandService flightCommandService) : IRequestHandler<DeleteFlightCommand, Unit>
    {
        private readonly IFlightCommandService _flightCommandService = flightCommandService;

        public async Task<Unit> Handle(DeleteFlightCommand command, CancellationToken cancellation)
        {
            await _flightCommandService.DeleteFlight(command.Id);
            return Unit.Value;
        }
    }
}
