using AutoMapper;
using FlightAPI.Controllers;
using FlightAPI.Models.DTOs;
using FlightAPI.Services.Interfaces;
using FlightAPI.Utility;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;


namespace FlightAPI.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly AuthController _authController;
        private readonly Mock<IAuthService> _mockAuthService;

        public AuthControllerTests()
        {
            _mockAuthService = new Mock<IAuthService>();
            _authController = new AuthController(_mockAuthService.Object);
        }

        [Fact]
        public async Task Register_ShouldReturnOkResult_WhenUserIsRegisteredSuccessfully()
        {
            // Arrange
            RegisterRequestDTO registerRequestDTO = new()
            {
                Email = "jet@gmail.com",
                FirstName = "Test",
                LastName = "Test",
                Role = Constants.Role_Admin,
                Password = "Test123@",
            };

            // Act
            var result = await _authController.Register(registerRequestDTO);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);
        }

        [Fact]
        public async Task Login_ShouldReturnCreatedAtActionResult_WhenUserLogsInSuccessfully()
        {
            // Arrange
            LoginRequestDTO loginRequestDTO = new()
            {
                Email = "jet@gmail.com",
                Password = "Test123@"
            };
            
            const string token = "3eQmpDMANAFw8IohOuLVr0U0Bz4CDEFe";

            _mockAuthService.Setup(service => service.LoginUser(loginRequestDTO))
                            .ReturnsAsync(new LoginResponseDTO { Token = token });
            // Act
            var result = await _authController.Login(loginRequestDTO);

            // Assert
            Assert.NotNull(result);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal((int)HttpStatusCode.Created, createdAtActionResult.StatusCode);

            var response = Assert.IsType<ApiResponse>(createdAtActionResult.Value);
            var loginResponse = Assert.IsType<LoginResponseDTO>(response.Result);
            Assert.Equal(token, loginResponse.Token);
        }

    }
}
