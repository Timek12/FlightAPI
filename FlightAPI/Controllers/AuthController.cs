using FlightAPI.Models;
using FlightAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using FlightAPI.Services.Interfaces;

namespace FlightAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly ApiResponse _response = new();

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequestDTO)
        {

            await _authService.RegisterUser(registerRequestDTO);
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            return Ok(_response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            _response.Result = await _authService.LoginUser(loginRequestDTO);
            _response.StatusCode = HttpStatusCode.Created;
            _response.IsSuccess = true;
            return CreatedAtAction(nameof(Login), _response);
        }
    }
}
