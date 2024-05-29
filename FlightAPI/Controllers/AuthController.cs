using FlightAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using FlightAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

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

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
            await _authService.RefreshToken(tokenModel);
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            return Ok(_response);
        }

        [Authorize]
        [HttpPost("revoke/{email}")]
        public async Task<IActionResult> Revoke(string email)
        {
            await _authService.Revoke(email);
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            return Ok(_response);
        }

        [Authorize]
        [HttpPost("revoke-all")]
        public async Task<IActionResult> RevokeAll()
        {
            await _authService.RevokeAll();
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            return Ok(_response);
        }
    }
}
