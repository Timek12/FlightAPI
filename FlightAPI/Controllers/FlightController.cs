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
            if (id <= 0)
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
                _response.Errors.Add("Flight with given id does not exists");
                _response.IsSuccess = false;
                return NotFound(_response);
            }

            _response.Result = flightDTO;
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFlightDTO flightDTO)
        {
            if (flightDTO is null || flightDTO.PlaneId <= 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Errors.Add("Invalid flight data");
                return BadRequest(_response);
            }

            var plane = await _db.Planes.FindAsync(flightDTO.PlaneId);
            if (plane is null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.Errors.Add("Plane with given id does not exists");
                return NotFound(_response);
            }

            Flight newFlight = new()
            {
                FlightNumber = flightDTO.FlightNumber,
                DepartureDate = flightDTO.DepartureDate,
                ArrivalLocation = flightDTO.ArrivalLocation,
                DepartureLocation = flightDTO.DepartureLocation,
                PlaneId = plane.Id
            };

            _db.Flights.Add(newFlight);
            await _db.SaveChangesAsync();

            _response.Result = _mapper.Map<FlightDTO>(newFlight);
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.Created;
            return CreatedAtAction(nameof(Get), new { id = newFlight.Id }, _response);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateFlightDTO flightDTO)
        {
            var flightFromDb = await _db.Flights.FindAsync(flightDTO.Id);
            if (flightFromDb is null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Errors.Add("Invalid flight data");
                return BadRequest(_response);
            }

            _mapper.Map(flightDTO, flightFromDb);

            await _db.SaveChangesAsync();

            _response.Result = _mapper.Map<FlightDTO>(flightFromDb);
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Errors.Add("Invalid flight id");
                return BadRequest(_response);
            }

            var flightFromDb = await _db.Flights.FindAsync(id);
            if (flightFromDb is null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Errors.Add("Flight with given id does not exists");
                return NotFound(_response);
            }

            _db.Flights.Remove(flightFromDb);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
