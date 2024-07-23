using MediatR;

namespace FlightAPI.Commands.DeleteFlightCommand
{
    public class DeleteFlightCommand(int id) : IRequest<Unit>
    {
        public int Id { get; } = id;
    }
}
