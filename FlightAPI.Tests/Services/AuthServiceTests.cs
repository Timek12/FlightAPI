using FlightAPI.Middleware;
using FlightAPI.Models;
using FlightAPI.Models.DTOs;
using FlightAPI.Repositories.Interfaces;
using FlightAPI.Services.Implementations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace FlightAPI.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly Mock<ILogger<ExceptionHandlingMiddleware>> _mockLogger;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockLogger = new Mock<ILogger<ExceptionHandlingMiddleware>>();

            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            var mockSecretKeySection = new Mock<IConfigurationSection>();
            mockSecretKeySection.SetupGet(m => m.Value).Returns("rKOAaacAwIJBrebML2DWZwmQTEBzbYRf");
            _mockConfiguration.Setup(a => a.GetSection("ApiSettings:SecretKey")).Returns(mockSecretKeySection.Object);

            var mockTokenExpirationDaysSection = new Mock<IConfigurationSection>();
            mockTokenExpirationDaysSection.SetupGet(m => m.Value).Returns("1");
            _mockConfiguration.Setup(a => a.GetSection("ApiSettings:TokenExpirationDays")).Returns(mockTokenExpirationDaysSection.Object);

            _authService = new AuthService(
                _mockUserRepository.Object,
                _mockConfiguration.Object,
                _mockUserManager.Object,
                _mockLogger.Object);
        }

        [Fact]
        public async Task LoginUser_ValidCredentials_ReturnsLoginResponseDTO()
        {
            // Arrange
            var loginRequest = new LoginRequestDTO { Email = "jet@example.com", Password = "Boeing123@" };
            var user = new ApplicationUser
            {
                Email = "jet@example.com",
                FirstName = "Test",
                LastName = "User",
                Id = Guid.NewGuid().ToString()
            };
            _mockUserRepository.Setup(x => x.GetUserByEmail(loginRequest.Email)).ReturnsAsync(user);
            _mockUserManager.Setup(x => x.CheckPasswordAsync(user, loginRequest.Password)).ReturnsAsync(true);
            var roles = new List<string> { "Admin" };
            _mockUserManager.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(roles);

            // Act
            var result = await _authService.LoginUser(loginRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(loginRequest.Email, result.Email);
            _mockUserRepository.Verify(x => x.GetUserByEmail(loginRequest.Email), Times.Once);
            _mockUserManager.Verify(x => x.CheckPasswordAsync(user, loginRequest.Password), Times.Once);
        }

        [Fact]
        public async Task RegisterUser_ValidRequest_CreatesUser()
        {
            // Arrange
            var registerRequest = new RegisterRequestDTO
            {
                Email = "jet@example.com",
                Password = "Boeing123@",
                FirstName = "Test",
                LastName = "User"
            };
            var user = new ApplicationUser
            {
                Email = registerRequest.Email,
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                UserName = registerRequest.Email,
                NormalizedEmail = registerRequest.Email.ToUpper()
            };

            _mockUserRepository.Setup(x => x.GetUserByEmail(registerRequest.Email)).ReturnsAsync((ApplicationUser)null);
            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), registerRequest.Password)).ReturnsAsync(IdentityResult.Success);
            _mockUserManager.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            // Act
            await _authService.RegisterUser(registerRequest);

            // Assert
            _mockUserRepository.Verify(x => x.GetUserByEmail(registerRequest.Email), Times.Once);
            _mockUserManager.Verify(x => x.CreateAsync(It.Is<ApplicationUser>(u => u.Email == user.Email), registerRequest.Password), Times.Once);
        }
    }
}
