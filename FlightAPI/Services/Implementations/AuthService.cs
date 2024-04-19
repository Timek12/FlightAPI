using FlightAPI.Exceptions;
using FlightAPI.Models;
using FlightAPI.Models.DTOs;
using FlightAPI.Repositories.Interfaces;
using FlightAPI.Services.Interfaces;
using FlightAPI.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FlightAPI.Services.Implementations
{
    public class AuthService(IAuthRepository authRepository, IConfiguration configuration,
        UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : IAuthService
    {
        private readonly IAuthRepository _authRepository = authRepository;
        private readonly string _secretKey = configuration.GetValue<string>("ApiSettings:SecretKey");
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;

        public async Task<LoginResponseDTO> LoginUser(LoginRequestDTO loginRequestDTO)
        {
            ApplicationUser? userFromDb = await _authRepository.GetUserByEmail(loginRequestDTO.Email);
            if (userFromDb is null)
            {
                throw new UserNotFoundException();

            }

            bool isValidPassword = await _userManager.CheckPasswordAsync(userFromDb, loginRequestDTO.Password);
            if (!isValidPassword)
            {
                throw new AuthenticationException();
            }

            JwtSecurityTokenHandler tokenhandler = new();
            byte[] key = Encoding.ASCII.GetBytes(_secretKey);
            var userRoles = await _userManager.GetRolesAsync(userFromDb);

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new ("firstName", userFromDb.FirstName),
                    new ("lastName", userFromDb.LastName),
                    new ("id", userFromDb.Id.ToString()),
                    new (ClaimTypes.Email, userFromDb.Email),
                    new (ClaimTypes.Role, userRoles.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken securityToken = tokenhandler.CreateToken(tokenDescriptor);

            LoginResponseDTO loginResponse = new()
            {
                Email = userFromDb.Email,
                Token = tokenhandler.WriteToken(securityToken),
            };

            if (string.IsNullOrEmpty(loginResponse.Token))
            {
                throw new FailedToGenerateTokenException();
            }

            return loginResponse;
        }

        public async Task RegisterUser(RegisterRequestDTO registerRequestDTO)
        {
            ApplicationUser? userFromDb = await _authRepository.GetUserByEmail(registerRequestDTO.Email);
            if (userFromDb is not null)
            {
                throw new UserNotFoundException();
            }

            ApplicationUser newUser = new()
            {
                FirstName = registerRequestDTO.FirstName,
                LastName = registerRequestDTO.LastName,
                Email = registerRequestDTO.Email,
                NormalizedEmail = registerRequestDTO.Email.ToUpper(),
                UserName = registerRequestDTO.Email,
            };

            var result = await _userManager.CreateAsync(newUser, registerRequestDTO.Password);

            if (!result.Succeeded)
            {
                throw new FailedToCreateUserException();
            }

            // For development purposes only

            if (registerRequestDTO.Role.Equals(Constants.Role_Admin, StringComparison.CurrentCultureIgnoreCase))
            {
                await _userManager.AddToRoleAsync(newUser, Constants.Role_Admin);
            }
            else
            {
                await _userManager.AddToRoleAsync(newUser, Constants.Role_Customer);
            }
        }
    }
}
