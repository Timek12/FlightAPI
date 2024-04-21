using AutoMapper;
using AutoMapper.QueryableExtensions;
using FlightAPI.Data;
using FlightAPI.Models;
using FlightAPI.Models.DTOs;
using FlightAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using FlightAPI.Exceptions;
using FlightAPI.Services.Implementations;

namespace FlightAPI.Repositories.Implementations
{
    public class FlightRepository(IApplicationDbContext db, IMapper mapper, ILogger<FlightRepository> logger) : IFlightRepository
    {
        private readonly IApplicationDbContext _db = db;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<FlightRepository> _logger = logger;

        public async Task<FlightDTO> Create(CreateFlightDTO flightDTO)
        {
            if(flightDTO is null)
            {
                _logger.LogError("Attempted to create a flight with null data");
                throw new NullFlightDataException();
            }

            Flight newFlight = new()
            {
                FlightNumber = flightDTO.FlightNumber,
                DepartureDate = flightDTO.DepartureDate,
                ArrivalLocation = flightDTO.ArrivalLocation,
                DepartureLocation = flightDTO.DepartureLocation,
                PlaneId = flightDTO.PlaneId,
            };

            _db.Flights.Add(newFlight);
            await _db.SaveChangesAsync();

            return _mapper.Map<FlightDTO>(newFlight);
        }

        public async Task Delete(Flight flight)
        {
            if (flight is null)
            {
                _logger.LogError("Attempted to delete a flight with null data");
                throw new NullFlightDataException();
            }

            _db.Flights.Remove(flight);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<FlightDTO>> GetAll()
        {
            return await _db.Flights
                .Include(f => f.Plane)
                .ProjectTo<FlightDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<Flight?> GetFlightById(int id)
        {
            return await _db.Flights.FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<FlightDTO?> GetFlightDTOById(int id)
        {
            return await _db.Flights
            .Include(f => f.Plane)
            .ProjectTo<FlightDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<FlightDTO> Update(UpdateFlightDTO flightDTO, Flight flight)
        {
            if(flight is null || flightDTO is null)
            {
                _logger.LogError("Attempted to update a flight with null data");
                throw new NullFlightDataException();
            }

            _mapper.Map(flightDTO, flight);
            _db.Flights.Update(flight);
            await _db.SaveChangesAsync();

            if (flight.PlaneId != null)
            {
                flight.Plane = await _db.Planes.FirstOrDefaultAsync(p => p.Id == flight.PlaneId);
            }

            return _mapper.Map<FlightDTO>(flight);
        }
    }
}
