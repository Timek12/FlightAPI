using FlightAPI.Controllers;
using FlightAPI.Models.DTOs;
using FlightAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FlightAPI.Tests.Controllers
{
    public class FlightControllerTests
    {
        private readonly Mock<IFlightCommandService> _mockFlightService;
        private readonly FlightController _flightController;

        public FlightControllerTests()
        {
            _mockFlightService = new Mock<IFlightCommandService>();
            _flightController = new FlightController(_mockFlightService.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WhenFlightsExist()
        {
            // Arrange
            var flights = new List<FlightDTO>
            {
                new() { Id = 1, FlightNumber = "LO111" },
                new() { Id = 2, FlightNumber = "LO222" },
                new() { Id = 2, FlightNumber = "LO333" },
            };

            _mockFlightService.Setup(service => service.GetAllFlights()).ReturnsAsync(flights);

            // Act
            var result = await _flightController.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse>(okResult.Value);
            var resultFlights = Assert.IsType<List<FlightDTO>>(apiResponse.Result);
            Assert.Equal(flights.Count, resultFlights.Count);
        }

        [Fact]
        public async Task Get_ReturnsOkResult_WhenFlightExists()
        {
            // Arrange
            var flights = new List<FlightDTO>
            {
                new() { Id = 1, FlightNumber = "LO111" },
                new() { Id = 2, FlightNumber = "LO222" },
                new() { Id = 3, FlightNumber = "LO333" },
            };

            var id = 1;
            var expectedFlight = flights.First(f => f.Id == id);
            _mockFlightService.Setup(service => service.GetFlightDTOById(id)).ReturnsAsync(expectedFlight);

            // Act
            var actionResult = await _flightController.Get(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var apiResponse = Assert.IsType<ApiResponse>(okResult.Value);
            var resultFlight = Assert.IsType<FlightDTO>(apiResponse.Result);
            Assert.Equal(expectedFlight.Id, resultFlight.Id);
            Assert.Equal(expectedFlight.FlightNumber, resultFlight.FlightNumber);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtActionResult_WhenFlightIsCreated()
        {
            // Arrange
            var flights = new List<FlightDTO>
            {
                new() { Id = 1, FlightNumber = "LO111" },
                new() { Id = 2, FlightNumber = "LO222" },
                new() { Id = 3, FlightNumber = "LO333" },
            };

            var createFlightDTO = new CreateFlightDTO
            {
                FlightNumber = "LO444",
                PlaneId = 1,
                ArrivalLocation = "Warsaw",
                DepartureLocation = "Phuket",
                DepartureDate = DateTime.Now,
            };

            var expectedFlight = new FlightDTO
            {
                Id = 4,
                FlightNumber = createFlightDTO.FlightNumber,
                ArrivalLocation = createFlightDTO.ArrivalLocation,
                DepartureLocation = createFlightDTO.DepartureLocation,
                DepartureDate = createFlightDTO.DepartureDate,
            };

            _mockFlightService.Setup(service => service.CreateFlight(createFlightDTO)).ReturnsAsync(expectedFlight);

            // Act
            var actionResult = await _flightController.Create(createFlightDTO);

            // Assert
            var createdAtResult = Assert.IsType<CreatedAtActionResult>(actionResult);
            var apiResponse = Assert.IsType<ApiResponse>(createdAtResult.Value);
            var resultFlight = Assert.IsType<FlightDTO>(apiResponse.Result);
            Assert.Equal(expectedFlight.Id, resultFlight.Id);
            Assert.Equal(expectedFlight.FlightNumber, resultFlight.FlightNumber);
        }

        [Fact]
        public async Task Update_ReturnsOkResult_WhenFlightIsUpdated()
        {
            // Arrange
            var id = 1;
            var updateFlightDTO = new UpdateFlightDTO
            {
                FlightNumber = "LO555",
                PlaneId = 2,
                ArrivalLocation = "Berlin",
                DepartureLocation = "Bangkok",
                DepartureDate = DateTime.Now.AddDays(1),
            };

            var expectedFlight = new FlightDTO
            {
                Id = id,
                FlightNumber = updateFlightDTO.FlightNumber,
                ArrivalLocation = updateFlightDTO.ArrivalLocation,
                DepartureLocation = updateFlightDTO.DepartureLocation,
                DepartureDate = updateFlightDTO.DepartureDate,
            };

            _mockFlightService.Setup(service => service.UpdateFlight(id, updateFlightDTO)).ReturnsAsync(expectedFlight);

            // Act
            var actionResult = await _flightController.Update(id, updateFlightDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var apiResponse = Assert.IsType<ApiResponse>(okResult.Value);
            var resultFlight = Assert.IsType<FlightDTO>(apiResponse.Result);
            Assert.Equal(expectedFlight.Id, resultFlight.Id);
            Assert.Equal(expectedFlight.FlightNumber, resultFlight.FlightNumber);
        }

        [Fact]
        public async Task Delete_ReturnsOkResult_WhenFlightIsDeleted()
        {
            // Arrange
            var id = 1;
            _mockFlightService.Setup(service => service.DeleteFlight(id)).Returns(Task.FromResult(true));
            // Act
            var actionResult = await _flightController.Delete(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var apiResponse = Assert.IsType<ApiResponse>(okResult.Value);
            Assert.True((bool)apiResponse.IsSuccess);
        }

    }
}
