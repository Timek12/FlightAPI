using FlightAPI.Models.DTOs;
using FlightAPI.Services.Interfaces;
using MediatR;

namespace FlightAPI.Commands.UpdateFlightCommand
{
    public class UpdateFlightHandler : IRequestHandler<UpdateFlightCommand, FlightDTO>
    {
        private readonly IFlightCommandService _flightCommandService;

        public UpdateFlightHandler(IFlightCommandService flightCommandService)
        {
            _flightCommandService = flightCommandService;
        }

        public async Task<FlightDTO> Handle(UpdateFlightCommand command, CancellationToken cancellation)
        {
            return await _flightCommandService.UpdateFlight(command.Id, command.UpdateFlightDTO);
        }
    }
}
