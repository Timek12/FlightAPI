using FlightAPI.Models.DTOs;
using MediatR;

namespace FlightAPI.Commands.UpdateFlightCommand
{
    public class UpdateFlightCommand : IRequest<FlightDTO>
    {
        public int Id { get; }
        public UpdateFlightDTO UpdateFlightDTO { get; }

        public UpdateFlightCommand(int id, UpdateFlightDTO updateFlightDTO)
        {
            Id = id;
            UpdateFlightDTO = updateFlightDTO;
        }
    }
}
