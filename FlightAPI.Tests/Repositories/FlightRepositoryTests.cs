using AutoMapper;
using Castle.Core.Logging;
using FlightAPI.Data;
using FlightAPI.Exceptions;
using FlightAPI.Models;
using FlightAPI.Models.DTOs;
using FlightAPI.Profiles;
using FlightAPI.Repositories.Implementations;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;


namespace FlightAPI.Tests.Repositories
{
    public class FlightRepositoryTests
    {
        private readonly FlightRepository _flightRepository;
        private readonly Mock<IApplicationDbContext> _mockDbContext;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<FlightRepository>> _mockLogger;

        public FlightRepositoryTests()
        {
            // Setup AutoMapper
            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile(new MappingProfile());
            });
            _mapper = config.CreateMapper();

            // Setup Logger
            _mockLogger = new Mock<ILogger<FlightRepository>>();

            // Setup DbContext
            _mockDbContext = new Mock<IApplicationDbContext>();

            var flights = new List<Flight>
            {
                new() { Id = 1, FlightNumber = "LO001" },
                new() { Id = 2, FlightNumber = "LO002" },
            };

            var planes = new List<Plane>
            {
                new() { Id = 1, Model = "A380", Type = "Airbus" },
                new() { Id = 2,  Model = "747", Type = "Boeing" }
            };

            var mockFlights = flights.AsQueryable().BuildMockDbSet();
            var planesFlights = planes.AsQueryable().BuildMockDbSet();

            _mockDbContext.Setup(f => f.Flights).Returns(mockFlights.Object);
            _mockDbContext.Setup(p => p.Planes).Returns(planesFlights.Object);

            _flightRepository = new FlightRepository(_mockDbContext.Object, _mapper, _mockLogger.Object);
        }

        [Fact]
        public async Task Create_ValidFlight_ReturnsFlightDTO()
        {
            // Arrange
            var flightDTO = new CreateFlightDTO { FlightNumber = "LO001" };

            // Act
            var result = await _flightRepository.Create(flightDTO);

            // Assert
            Assert.Equal(flightDTO.FlightNumber, result.FlightNumber);
            _mockDbContext.Verify(f => f.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Create_DatabaseFailure_ThrowsException()
        {
            // Arrange
            var flightDTO = new CreateFlightDTO { FlightNumber = "LO001" };
            _mockDbContext.Setup(f => f.SaveChangesAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _flightRepository.Create(flightDTO));
        }

        [Fact]
        public async Task Create_NullInput_ThrowsException()
        {
            // Arrange
            CreateFlightDTO flightDTO = null;

            // Act & Assert
            await Assert.ThrowsAsync<NullFlightDataException>(() => _flightRepository.Create(flightDTO));
        }

        [Fact]
        public async Task Delete_ValidFlight_RemovesFlight()
        {
            // Arrange
            var flight = new Flight { Id = 1, FlightNumber = "LO001" };

            // Act
            await _flightRepository.Delete(flight);

            // Assert
            _mockDbContext.Verify(f => f.Flights.Remove(flight), Times.Once);
            _mockDbContext.Verify(f => f.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Delete_NullInput_ThrowsException()
        {
            // Arrange
            Flight flight = null;

            // Act & Assert
            await Assert.ThrowsAsync<NullFlightDataException>(() => _flightRepository.Delete(flight));
        }

        [Fact]
        public async Task GetAll_ReturnsAllFlights()
        {
            // Act
            var result = await _flightRepository.GetAll();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetFlightById_ValidId_ReturnsFlight()
        {
            // Arrange
            var id = 1;

            // Act
            var result = await _flightRepository.GetFlightById(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
        }

        [Fact]
        public async Task GetFlightById_InvalidId_ReturnsNull()
        {
            // Arrange
            var id = 999; // This ID doesn't exist in the database

            // Act
            var result = await _flightRepository.GetFlightById(id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetFlightDTOById_ValidId_ReturnsFlightDTO()
        {
            // Arrange
            var id = 1;

            // Act
            var result = await _flightRepository.GetFlightDTOById(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
        }

        [Fact]
        public async Task GetFlightDTOById_InvalidId_ReturnsNull()
        {
            // Arrange
            var id = 999; // This ID doesn't exist in the database

            // Act
            var result = await _flightRepository.GetFlightDTOById(id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Update_ValidFlight_ReturnsUpdatedFlightDTO()
        {
            // Arrange
            var flight = new Flight { Id = 1, FlightNumber = "LO001" };
            var flightDTO = new UpdateFlightDTO { FlightNumber = "LO003" };

            // Act
            var result = await _flightRepository.Update(flightDTO, flight);

            // Assert
            Assert.Equal(flightDTO.FlightNumber, result.FlightNumber);
            _mockDbContext.Verify(f => f.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Update_NullFlightDTO_ThrowsException()
        {
            // Arrange
            var flight = new Flight { Id = 1, FlightNumber = "LO001" };
            UpdateFlightDTO flightDTO = null;

            // Act & Assert
            await Assert.ThrowsAsync<NullFlightDataException>(() => _flightRepository.Update(flightDTO, flight));
        }

        [Fact]
        public async Task Update_NullFlight_ThrowsException()
        {
            // Arrange
            var flightDTO = new UpdateFlightDTO { FlightNumber = "LO003" };
            Flight flight = null;

            // Act & Assert
            await Assert.ThrowsAsync<NullFlightDataException>(() => _flightRepository.Update(flightDTO, flight));
        }
    }
}
