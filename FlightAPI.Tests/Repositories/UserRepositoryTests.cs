using FlightAPI.Data;
using FlightAPI.Models;
using FlightAPI.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using Moq.EntityFrameworkCore;
using System.Net.Sockets;

namespace FlightAPI.Tests.Repositories
{
    public class UserRepositoryTests
    {
        private readonly UserRepository _userRepository;
        private readonly Mock<IApplicationDbContext> _mockDbContext;

        public UserRepositoryTests()
        {
            _mockDbContext = new Mock<IApplicationDbContext>();

            var users = new List<ApplicationUser>
            {
                new() { Email = "test1@gmail.com" },
                new() { Email = "test2@gmail.com" },
            };

            var mockSet = users.AsQueryable().BuildMockDbSet();
            _mockDbContext.Setup(u => u.ApplicationUsers).Returns(mockSet.Object);
            _userRepository = new UserRepository(_mockDbContext.Object);
        }

        [Fact]
        public async Task GetUserByEmail_UserExists_ReturnsCorrectUser()
        {
            // Arrange
            var email = "test2@gmail.com";

            // Act
            var result = await _userRepository.GetUserByEmail(email);

            // Assert
            Assert.Equal(email, result.Email);
        }

        [Fact]
        public async Task GetUserByEmail_UserDoesNotExist_ReturnsNull()
        {
            // Arrange
            var email = "invalid@gmail.com";

            // Act
            var result = await _userRepository.GetUserByEmail(email);

            // Assert 
            Assert.Null(result);
            if (result != null)
            {
                Assert.NotEqual(email, result.Email);
            }
        }
    }
}
