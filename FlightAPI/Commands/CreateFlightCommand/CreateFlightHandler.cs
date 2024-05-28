using FlightAPI.Models.DTOs;
using FlightAPI.Services.Interfaces;
using MediatR;

namespace FlightAPI.Commands.CreateFlightCommand
{
    public class CreateFlightHandler : IRequestHandler<CreateFlightCommand, FlightDTO>
    {
        private readonly IFlightCommandService _flightCommandService;

        public CreateFlightHandler(IFlightCommandService flightCommandService)
        {
            _flightCommandService = flightCommandService;
        }

        public async Task<FlightDTO> Handle(CreateFlightCommand command, CancellationToken cancellation)
        {
            return await _flightCommandService.CreateFlight(command.CreateFlightDTO);
        }
    }
}
