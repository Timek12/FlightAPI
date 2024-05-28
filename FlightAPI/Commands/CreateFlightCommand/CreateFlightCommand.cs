using FlightAPI.Models.DTOs;
using MediatR;

namespace FlightAPI.Commands.CreateFlightCommand
{
    public class CreateFlightCommand : IRequest<FlightDTO>
    {
        public CreateFlightDTO CreateFlightDTO { get; }

        public CreateFlightCommand(CreateFlightDTO createFlightDTO)
        {
            CreateFlightDTO = createFlightDTO; 
        }
    }
}
