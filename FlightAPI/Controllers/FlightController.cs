using FlightAPI.Models;
using FlightAPI.Models.DTOs;
using FlightAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
            _response.Result = await _flightService.GetAllFlights();
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            return Ok(_response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            _response.Result = await _flightService.GetFlightDTOById(id); ;
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFlightDTO flightDTO)
        {
            var newFlightDTO = await _flightService.CreateFlight(flightDTO);
            _response.Result = newFlightDTO;
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.Created;
            return CreatedAtAction(nameof(Get), new { id = newFlightDTO.Id }, _response);
        }

        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateFlightDTO flightDTO)
        {
            _response.Result = await _flightService.UpdateFlight(id, flightDTO);
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _flightService.DeleteFlight(id);
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
    }
}
