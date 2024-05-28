using MediatR;

namespace FlightAPI.Commands.DeleteFlightCommand
{
    public class DeleteFlightCommand : IRequest<Unit>
    {
        public int Id { get; }

        public DeleteFlightCommand(int id)
        {
            Id = id;
        }
    }
}
