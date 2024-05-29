using AutoMapper;
using FlightAPI.Data;
using FlightAPI.Exceptions;
using FlightAPI.Models;
using FlightAPI.Models.DTOs;
using FlightAPI.Repositories.Interfaces;

namespace FlightAPI.Repositories.Implementations
{
    public class PlaneRepository : IPlaneRepository
    {
        private readonly IApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly ILogger<PlaneRepository> _logger;

        public PlaneRepository(IApplicationDbContext db, IMapper mapper, ILogger<PlaneRepository> logger)
        {
            _db = db;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PlaneDTO> CreatePlane(CreatePlaneDTO createPlaneDTO)
        {
            if(createPlaneDTO is null)
            {
                _logger.LogError("Attempted to create a plane with null data");
                throw new NullPlaneDataException();
            }

            Plane newPlane = new Plane()
            {
                Model = createPlaneDTO.Model,
                Type = createPlaneDTO.Type,
                Manufacturer = createPlaneDTO.Manufacturer,
                Capacity = createPlaneDTO.Capacity,
                CruiseSpeed = createPlaneDTO.CruiseSpeed,
                Range = createPlaneDTO.Range
            };

            _db.Planes.Add(newPlane);
            await _db.SaveChangesAsync();

            return _mapper.Map<PlaneDTO>(newPlane);
        }

        public async void DeletePlane(Plane plane)
        {
            if(plane is null)
            {
                _logger.LogError("Attempted to delete a plane with null data");
                throw new NullPlaneDataException();
            }

            _db.Planes.Remove(plane);
            await _db.SaveChangesAsync();
        }

        public async Task<PlaneDTO> UpdatePlane(UpdatePlaneDTO updatePlaneDTO, Plane plane)
        {
            if(updatePlaneDTO is null || plane is null)
            {
                _logger.LogError("Attempted to update a plane with null data");
                throw new NullPlaneDataException();
            }

            _mapper.Map(updatePlaneDTO, plane);

            _db.Planes.Update(plane);
            await _db.SaveChangesAsync();

            return _mapper.Map<PlaneDTO>(plane);
        }
    }
}
