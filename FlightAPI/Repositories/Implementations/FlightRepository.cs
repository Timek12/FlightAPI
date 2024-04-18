using AutoMapper;
using AutoMapper.QueryableExtensions;
using FlightAPI.Data;
using FlightAPI.Models;
using FlightAPI.Models.DTOs;
using FlightAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlightAPI.Repositories.Implementations
{
    public class FlightRepository(ApplicationDbContext db, IMapper mapper) : IFlightRepository
    {
        private readonly ApplicationDbContext _db = db;
        private readonly IMapper _mapper = mapper;

        public async Task<FlightDTO> Create(CreateFlightDTO flightDTO)
        {
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
            return await _db.Flights.FindAsync(id);
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
            _mapper.Map(flightDTO, flight);
            _db.Flights.Update(flight);
            await _db.SaveChangesAsync();
            flight.Plane = await _db.Planes.FindAsync(flight.PlaneId);

            return _mapper.Map<FlightDTO>(flight);
        }
    }
}
