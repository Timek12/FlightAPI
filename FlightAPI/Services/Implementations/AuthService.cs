using FlightAPI.Exceptions;
using FlightAPI.Middleware;
using FlightAPI.Models;
using FlightAPI.Models.DTOs;
using FlightAPI.Repositories.Interfaces;
using FlightAPI.Services.Interfaces;
using FlightAPI.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FlightAPI.Services.Implementations
{
    public class AuthService(
        IUserRepository userRepository,
        IConfiguration configuration,
        UserManager<ApplicationUser> userManager,
        ILogger<ExceptionHandlingMiddleware> logger) : IAuthService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;
        private readonly IConfiguration _configuration = configuration;
        private readonly string _secretKey = configuration?.GetValue<string>("JwtSettings:Secret") ?? string.Empty;
        private readonly int _tokenExpirationDays = configuration.GetValue<int>("JwtSettings:TokenExpirationDays");

        public async Task<LoginResponseDTO> LoginUser(LoginRequestDTO loginRequestDTO)
        {
            _logger.LogInformation($"User with email: {loginRequestDTO.Email} attempting to log in.");
            ApplicationUser? userFromDb = await _userManager.FindByEmailAsync(loginRequestDTO.Email);
            if (userFromDb is null)
            {
                throw new UserNotFoundException();
            }

            bool isValidPassword = await _userManager.CheckPasswordAsync(userFromDb, loginRequestDTO.Password);
            if (!isValidPassword)
            {
                throw new AuthenticationException();
            }

            var userRoles = await _userManager.GetRolesAsync(userFromDb);

            var authClaims = new List<Claim>
            {
                new(ClaimTypes.Email, userFromDb?.Email ?? string.Empty),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = GenerateJwtToken(authClaims) ?? throw new FailedToGenerateTokenException();
            var refreshToken = GenerateRefreshToken() ?? throw new FailedToGenerateTokenException();

            _ = int.TryParse(_configuration["JwtSettings:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

            userFromDb.RefreshToken = refreshToken;
            userFromDb.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

            await _userManager.UpdateAsync(userFromDb);

            LoginResponseDTO loginResponseDTO = new()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                Expiration = token.ValidTo
            };

            return loginResponseDTO;
        }

        public async Task RegisterUser(RegisterRequestDTO registerRequestDTO)
        {
            _logger.LogInformation($"Registering user with email: {registerRequestDTO.Email}.");
            ApplicationUser? userFromDb = await _userRepository.GetUserByEmail(registerRequestDTO.Email);
            if (userFromDb is not null)
            {
                throw new UserAlreadyExistsException();
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
                var errorMessage = string.Join(", ", result.Errors.Select(x => x.Description));
                throw new FailedToCreateUserException($"Failed to create a new user: {errorMessage}");
            }

            // For development purposes only

            if (registerRequestDTO.Role != null && registerRequestDTO.Role.Equals(Constants.Role_Admin, StringComparison.CurrentCultureIgnoreCase))
            {
                await _userManager.AddToRoleAsync(newUser, Constants.Role_Admin);
            }
            else
            {
                await _userManager.AddToRoleAsync(newUser, Constants.Role_Customer);
            }
        }

        public async Task<TokenModel> RefreshToken(TokenModel tokenModel)
        {
            if (tokenModel is null)
            {
                throw new InvalidRefreshTokenException();
            }

            string? accessToken = tokenModel.AccessToken;
            string? refreshToken = tokenModel.RefreshToken;

            var principal = GetPrincipalFromExpiredToken(accessToken);
            if (principal is null)
            {
                throw new InvalidAccessTokenException();
            }

            string email = principal?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (email == null)
            {
                throw new AuthenticationException();
            }

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                throw new InvalidRefreshTokenException();
            }

            var newAccessToken = GenerateJwtToken(principal.Claims.ToList());
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);

            TokenModel token = new()
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                RefreshToken = newRefreshToken
            };

            return token;
        }

        public async Task Revoke(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                throw new UserNotFoundException();
            }

            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);
        }

        public async Task RevokeAll()
        {
            var users = _userManager.Users.ToList();
            foreach (var user in users)
            {
                user.RefreshToken = null;
                await _userManager.UpdateAsync(user);
            }
        }

        private SecurityToken GenerateJwtToken(List<Claim> authClaims)
        {
            JwtSecurityTokenHandler tokenhandler = new();
            byte[] key = Encoding.ASCII.GetBytes(_secretKey);
            _ = int.TryParse(_configuration["JwtSettins:TokenValidityInMinutes"], out int tokenValidityInMinutes);
            IDictionary<string, object> claimsDictionary = authClaims.ToDictionary(claim => claim.Type, claim => (object)claim.Value);

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Issuer = _configuration["JwtSettings:ValidIssuer"],
                Audience = _configuration["JwtSettings:ValidAudience"],
                Subject = new ClaimsIdentity(authClaims),
                Expires = DateTime.UtcNow.AddDays(_tokenExpirationDays),
                Claims = claimsDictionary,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken securityToken = tokenhandler.CreateToken(tokenDescriptor);
            return securityToken;
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration?["JwtSettings:Secret"] ?? string.Empty)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;

        }
    }
}