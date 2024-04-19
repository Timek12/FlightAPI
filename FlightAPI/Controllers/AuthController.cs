using FlightAPI.Models;
using FlightAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using FlightAPI.Services.Interfaces;
using FlightAPI.Exceptions;

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
            try
            {
                await _authService.RegisterUser(registerRequestDTO);
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (UserNotFoundException ex)
            {
                _response.Errors.Add(ex.Message);
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                return NotFound(_response);
            }
            catch (FailedToCreateUserException ex)
            {
                _response.Errors.Add(ex.Message);
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }

            return BadRequest(_response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            try
            {
                _response.Result = await _authService.LoginUser(loginRequestDTO);
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (InvalidPasswordException ex)
            {
                _response.Errors.Add(ex.Message);
                _response.StatusCode = HttpStatusCode.Unauthorized;
                _response.IsSuccess = false;
                return Unauthorized(_response);
            }
            catch (UserNotFoundException ex)
            {
                _response.Errors.Add(ex.Message);
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                return NotFound(_response);
            }
            catch (FailedToGenerateTokenException ex)
            {
                _response.Errors.Add(ex.Message);
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }

        }
    }
}
