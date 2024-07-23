using FlightAPI.Commands.CreatePlaneCommand;
using FlightAPI.Commands.DeletePlaneCommand;
using FlightAPI.Commands.UpdatePlaneCommand;
using FlightAPI.Models.DTOs;
using FlightAPI.Queries.GetAllPlanesQuery;
using FlightAPI.Queries.GetPlaneByIdQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FlightAPI.Controllers
{
    [Route("/api/Planes")]
    [ApiController]
    public class PlaneController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;
        private readonly ApiResponse _response = new();

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _response.Result = _mediator.Send(new GetAllPlanesQuery());
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            return Ok(_response);
        }

        [HttpGet("{id}:int")]
        public async Task<IActionResult> GetById(int id)
        {
            _response.Result = _mediator.Send(new GetPlaneByIdQuery(id));
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            return Ok(_response);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePlaneDTO createPlaneDTO)
        {
            _response.Result = _mediator.Send(new CreatePlaneCommand(createPlaneDTO));
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            return Ok(_response);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdatePlaneDTO updatePlaneDTO)
        {
            _response.Result = _mediator.Send(new UpdatePlaneCommand(updatePlaneDTO));
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            return Ok(_response);
        }

        [Authorize]
        [HttpDelete("{id}:int")]
        public async Task<IActionResult> Delete(int id)
        {
            _response.Result = _mediator.Send(new DeletePlaneCommand(id));
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            return Ok(_response);
        }
    }
}
