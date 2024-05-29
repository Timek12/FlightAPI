using MediatR;

namespace FlightAPI.Commands.DeletePlaneCommand
{
    public class DeletePlaneCommand : IRequest<Unit> 
    {
        public int Id { get; }

        public DeletePlaneCommand(int id)
        {
            Id = id;
        }
    }
}
