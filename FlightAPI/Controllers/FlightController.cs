using FlightAPI.Commands.CreateFlightCommand;
using FlightAPI.Commands.DeleteFlightCommand;
using FlightAPI.Commands.UpdateFlightCommand;
using FlightAPI.Models.DTOs;
using FlightAPI.Queries.GetAllFlightsQuery;
using FlightAPI.Queries.GetFlightByIdQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FlightAPI.Controllers
{
    [Route("api/flights")]
    [ApiController]
       public class FlightController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly ApiResponse _response = new();

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _response.Result = await _mediator.Send(new GetAllFlightsQuery());
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            return Ok(_response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            _response.Result = await _mediator.Send(new GetFlightByIdQuery(id));
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateFlightDTO flightDTO)
        {
            var newFlightDTO = await _mediator.Send(new CreateFlightCommand(flightDTO));
            _response.Result = newFlightDTO;
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.Created;
            return CreatedAtAction(nameof(Get), new { id = newFlightDTO.Id }, _response);
        }

        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateFlightDTO flightDTO)
        {
            _response.Result = await _mediator.Send(new UpdateFlightCommand(id, flightDTO));
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteFlightCommand(id));
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
    }
}
