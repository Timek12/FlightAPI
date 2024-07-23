using FlightAPI.Models.DTOs;
using MediatR;

namespace FlightAPI.Commands.CreateFlightCommand
{
    public class CreateFlightCommand(CreateFlightDTO createFlightDTO) : IRequest<FlightDTO>
    {
        public CreateFlightDTO CreateFlightDTO { get; } = createFlightDTO;
    }
}
