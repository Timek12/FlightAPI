using MediatR;

namespace FlightAPI.Commands.DeletePlaneCommand
{
    public class DeletePlaneCommand(int id) : IRequest<Unit> 
    {
        public int Id { get; } = id;
    }
}
