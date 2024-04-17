using FlightAPI.Data;
using FlightAPI.Models;
using FlightAPI.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using FlightAPI.Utility;

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

        public AuthController(ApplicationDbContext db, string secretKey, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _response = new ApiResponse();
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
    }
}
