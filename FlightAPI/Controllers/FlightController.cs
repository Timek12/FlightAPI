using AutoMapper;
using AutoMapper.QueryableExtensions;
using FlightAPI.Data;
using FlightAPI.Models;
using FlightAPI.Models.DTOs;
using FlightAPI.Utility.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace FlightAPI.Controllers
{
    [Route("api/flights")]
    [ApiController]
    public class FlightController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private ApiResponse _response;

        public FlightController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ApiResponse();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _response.Result = await _db.Flights
                .Include(f => f.Plane)
                .ProjectTo<FlightDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            return Ok(_response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            if(id <= 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Errors.Add("Incorrect flight id");
                _response.IsSuccess = false;
                return BadRequest(_response);
            }

            FlightDTO? flightDTO = await _db.Flights
                .Include(f => f.Plane)
                .ProjectTo<FlightDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (flightDTO is null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Errors.Add("Flight with given id does not exists.");
                _response.IsSuccess = false;
                return NotFound(_response);
            }

            _response.Result = flightDTO;
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
    }
}
