using FlightAPI.Models;
using FlightAPI.Models.DTOs;

namespace FlightAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task  RegisterUser(RegisterRequestDTO registerRequestDTO);
        Task<LoginResponseDTO> LoginUser(LoginRequestDTO loginRequestDTO);
        Task<TokenModel> RefreshToken(TokenModel tokenModel);
        Task Revoke(string email);
        Task RevokeAll();
    }
}
