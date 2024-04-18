using FlightAPI.Models;
using FlightAPI.Models.DTOs;
using FlightAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FlightAPI.Controllers
{
    [Route("api/flights")]
    [ApiController]
    public class FlightController(IFlightService flightService) : ControllerBase
    {
        private readonly IFlightService _flightService = flightService;
        private readonly ApiResponse _response = new();

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _response.Result = await _flightService.GetAllFlights();
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (KeyNotFoundException ex)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.Errors.Add(ex.Message);
                _response.IsSuccess = false;
                return NotFound(_response);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                _response.Result = await _flightService.GetFlightDTOById(id); ;
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (ArgumentException ex)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Errors.Add(ex.Message);
                _response.IsSuccess = false;
                return BadRequest(_response);
            }
            catch (KeyNotFoundException ex)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.Errors.Add(ex.Message);
                _response.IsSuccess = false;
                return NotFound(_response);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFlightDTO flightDTO)
        {
            try
            {
                var newFlightDTO = await _flightService.CreateFlight(flightDTO);
                _response.Result = newFlightDTO;
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtAction(nameof(Get), new { id = newFlightDTO.Id }, _response);
            }
            catch (ArgumentNullException ex)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Errors.Add(ex.Message);
                _response.IsSuccess = false;
                return BadRequest(_response);
            }
            catch (ArgumentException ex)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Errors.Add(ex.Message);
                _response.IsSuccess = false;
                return BadRequest(_response);
            }
            catch (KeyNotFoundException ex)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.Errors.Add(ex.Message);
                _response.IsSuccess = false;
                return NotFound(_response);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateFlightDTO flightDTO)
        {
            try
            {
                _response.Result = await _flightService.UpdateFlight(id, flightDTO);
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (ArgumentNullException ex)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Errors.Add(ex.Message);
                _response.IsSuccess = false;
                return BadRequest(_response);
            }
            catch (ArgumentException ex)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Errors.Add(ex.Message);
                _response.IsSuccess = false;
                return BadRequest(_response);
            }
            catch (KeyNotFoundException ex)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.Errors.Add(ex.Message);
                _response.IsSuccess = false;
                return NotFound(_response);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _flightService.Delete(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Errors.Add(ex.Message);
                _response.IsSuccess = false;
                return BadRequest(_response);
            }
            catch (KeyNotFoundException ex)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.Errors.Add(ex.Message);
                _response.IsSuccess = false;
                return NotFound(_response);
            }
        }
    }
}
