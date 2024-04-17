using FlightAPI.Data;
using FlightAPI.Models;
using FlightAPI.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using FlightAPI.Utility;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace FlightAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private ApiResponse _response;
        private string secretKey;

        public AuthController(ApplicationDbContext db, IConfiguration configuration, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _response = new ApiResponse();
            secretKey = configuration.GetValue<string>("ApiSettings:SecretKey");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequestDTO)
        {
            try
            {
                ApplicationUser? userFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == registerRequestDTO.Email.ToLower());
                if (userFromDb is not null)
                {
                    _response.Errors.Add("User already exists.");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
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
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    foreach (var error in result.Errors)
                    {
                        _response.Errors.Add(error.Description);
                    }

                    return BadRequest(_response);
                }

                if (!_roleManager.RoleExistsAsync(Constants.Role_Admin).GetAwaiter().GetResult())
                {
                    // Create roles in database
                    await _roleManager.CreateAsync(new IdentityRole(Constants.Role_Admin));
                    await _roleManager.CreateAsync(new IdentityRole(Constants.Role_Customer));
                }

                if (registerRequestDTO.Role.Equals(Constants.Role_Admin, StringComparison.CurrentCultureIgnoreCase))
                {
                    await _userManager.AddToRoleAsync(newUser, Constants.Role_Admin);
                }
                else
                {
                    await _userManager.AddToRoleAsync(newUser, Constants.Role_Customer);
                }

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.Errors.Add(ex.Message);
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
            }

            return BadRequest(_response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            ApplicationUser? userFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == loginRequestDTO.Email.ToLower());
            if (userFromDb is null)
            {
                _response.Result = new LoginResponseDTO();
                _response.StatusCode=HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Errors.Add("User does not exists.");
                return BadRequest(_response);
            }

            bool isValidPassword = await _userManager.CheckPasswordAsync(userFromDb, loginRequestDTO.Password);
            if(!isValidPassword)
            {
                _response.Result = new LoginResponseDTO();
                _response.StatusCode=HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Errors.Add("Email or password is incorrect.");
                return BadRequest(_response);
            }

            JwtSecurityTokenHandler tokenhandler = new();
            byte[] key = Encoding.ASCII.GetBytes(secretKey);
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

            if(string.IsNullOrEmpty(loginResponse.Token))
            {
                _response.Errors.Add("Failed to genereate token.");
                _response.Result = new LoginResponseDTO();
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            _response.Result = loginResponse;
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            return Ok(_response);
        }
    }
}
