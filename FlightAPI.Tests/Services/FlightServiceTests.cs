using FlightAPI.Exceptions;
using FlightAPI.Middleware;
using FlightAPI.Models;
using FlightAPI.Models.DTOs;
using FlightAPI.Repositories.Interfaces;
using FlightAPI.Services.Implementations;
using Microsoft.Extensions.Logging;
using Moq;

namespace FlightAPI.Tests.Services
{
    public class FlightServiceTests
    {
        private readonly Mock<IFlightRepository> _mockFlightRepository;
        private readonly Mock<IPlaneRepository> _mockPlaneRepository;
        private readonly Mock<ILogger<ExceptionHandlingMiddleware>> _mockLogger;
        private readonly FlightService _flightService;

        public FlightServiceTests()
        {
            _mockFlightRepository = new Mock<IFlightRepository>();
            _mockPlaneRepository = new Mock<IPlaneRepository>();
            _mockLogger = new Mock<ILogger<ExceptionHandlingMiddleware>>();
            _flightService = new FlightService(_mockFlightRepository.Object, _mockPlaneRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task CreateFlight_ValidRequest_CreatesFlight()
        {
            // Arrange
            var createFlightDTO = new CreateFlightDTO
            {
                FlightNumber = "LO123",
                PlaneId = 1
            };

            var plane = new Plane { Id = 1 };
            var flight = new FlightDTO
            {
                FlightNumber = "LO123",
                Plane = new PlaneDTO
                {
                    Model = "Boeing",
                    Type = "737"
                }
            };
            _mockPlaneRepository.Setup(x => x.GetPlaneById(createFlightDTO.PlaneId)).ReturnsAsync(plane);
            _mockFlightRepository.Setup(x => x.Create(createFlightDTO)).ReturnsAsync(flight);

            // Act
            var result = await _flightService.CreateFlight(createFlightDTO);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(createFlightDTO.FlightNumber, result.FlightNumber);
            _mockPlaneRepository.Verify(x => x.GetPlaneById(createFlightDTO.PlaneId), Times.Once);
            _mockFlightRepository.Verify(x => x.Create(createFlightDTO), Times.Once);
        }

        [Fact]
        public async Task CreateFlight_NullRequest_ThrowsException()
        {
            // Arrange
            CreateFlightDTO createFlightDTO = null;

            // Act & Assert
            await Assert.ThrowsAsync<NullFlightDataException>(() => _flightService.CreateFlight(createFlightDTO));
        }

        [Fact]
        public async Task CreateFlight_InvalidPlaneId_ThrowsException()
        {
            // Arrange
            var createFlightDTO = new CreateFlightDTO
            {
                FlightNumber = "LO123",
                PlaneId = -1
            };
            _mockPlaneRepository.Setup(x => x.GetPlaneById(createFlightDTO.PlaneId)).ReturnsAsync((Plane)null);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidPlaneIdException>(() => _flightService.CreateFlight(createFlightDTO));
        }

        [Fact]
        public async Task DeleteFlight_InvalidId_ThrowsException()
        {
            // Arrange
            int id = -1;

            // Act & Assert
            await Assert.ThrowsAsync<InvalidFlightIdException>(() => _flightService.DeleteFlight(id));
        }

        [Fact]
        public async Task DeleteFlight_FlightNotFound_ThrowsException()
        {
            // Arrange
            int id = 1;
            _mockFlightRepository.Setup(x => x.GetFlightById(id)).ReturnsAsync((Flight)null);
            // Act & Assert
            await Assert.ThrowsAsync<FlightNotFoundException>(() => _flightService.DeleteFlight(id));
        }

        [Fact]
        public async Task GetAllFlights_ReturnsAllFlights()
        {
            // Arrange
            var flights = new List<FlightDTO>
            {
                new() {
                    FlightNumber = "LO123",
                    Plane = new PlaneDTO { Model = "Boeing", Type = "737" }
                },
                new() { FlightNumber = "LO456",
                    Plane = new PlaneDTO { Model = "Airbus", Type = "A380" }
                }
            };
            _mockFlightRepository.Setup(x => x.GetAll()).ReturnsAsync(flights);

            // Act
            var result = await _flightService.GetAllFlights();

            // Assert
            Assert.Equal(flights, result);
            _mockFlightRepository.Verify(x => x.GetAll(), Times.Once);
        }

        [Fact]
        public async Task GetFlightDTOById_InvalidId_ThrowsException()
        {
            // Arrange
            int id = -1;

            // Act & Assert
            await Assert.ThrowsAsync<InvalidFlightIdException>(() => _flightService.GetFlightDTOById(id));
        }

        [Fact]
        public async Task GetFlightDTOById_FlightNotFound_ThrowsException()
        {
            // Arrange
            int id = 1;
            _mockFlightRepository.Setup(x => x.GetFlightDTOById(id)).ReturnsAsync((FlightDTO)null);

            // Act & Assert
            await Assert.ThrowsAsync<FlightNotFoundException>(() => _flightService.GetFlightDTOById(id));
        }

        [Fact]
        public async Task UpdateFlight_NullRequest_ThrowsException()
        {
            // Arrange
            int id = 1;
            UpdateFlightDTO updateFlightDTO = null;

            // Act & Assert
            await Assert.ThrowsAsync<NullFlightDataException>(() => _flightService.UpdateFlight(id, updateFlightDTO));
        }

        [Fact]
        public async Task UpdateFlight_InvalidId_ThrowsException()
        {
            // Arrange
            int id = -1;
            var updateFlightDTO = new UpdateFlightDTO { Id = -1, FlightNumber = "LO123", PlaneId = 1 };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidFlightIdException>(() => _flightService.UpdateFlight(id, updateFlightDTO));
        }

        [Fact]
        public async Task UpdateFlight_FlightNotFound_ThrowsException()
        {
            // Arrange
            int id = 1;
            var updateFlightDTO = new UpdateFlightDTO { Id = 1, FlightNumber = "LO123", PlaneId = 1 };
            _mockFlightRepository.Setup(x => x.GetFlightById(updateFlightDTO.Id)).ReturnsAsync((Flight)null);

            // Act & Assert
            await Assert.ThrowsAsync<FlightNotFoundException>(() => _flightService.UpdateFlight(id, updateFlightDTO));
        }

    }
}
