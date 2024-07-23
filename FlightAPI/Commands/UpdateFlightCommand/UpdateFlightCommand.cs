using FlightAPI.Models.DTOs;
using MediatR;

namespace FlightAPI.Commands.UpdateFlightCommand
{
    public class UpdateFlightCommand(int id, UpdateFlightDTO updateFlightDTO) : IRequest<FlightDTO>
    {
        public int Id { get; } = id;
        public UpdateFlightDTO UpdateFlightDTO { get; } = updateFlightDTO;
    }
}
