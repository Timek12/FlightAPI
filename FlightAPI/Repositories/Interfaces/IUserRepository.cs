using FlightAPI.Models;

namespace FlightAPI.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<ApplicationUser?> GetUserByEmail(string email);
    }
}
