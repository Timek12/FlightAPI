using FlightAPI.Models;

namespace FlightAPI.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<ApplicationUser?> GetUserByEmail(string email);
    }
}
