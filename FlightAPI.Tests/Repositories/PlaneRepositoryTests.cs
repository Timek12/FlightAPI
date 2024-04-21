using Moq;
using Microsoft.EntityFrameworkCore;
using FlightAPI.Repositories.Implementations;
using FlightAPI.Models;
using FlightAPI.Data;

namespace FlightAPI.Tests.Repositories
{
    public class PlaneRepositoryTests
    {
        private readonly PlaneRepository _planeRepository;
        private readonly Mock<DbSet<Plane>> _mockSet;
        private readonly Mock<IApplicationDbContext> _mockContext;

        public PlaneRepositoryTests()
        {
            _mockContext = new Mock<IApplicationDbContext>();
            _mockSet = new Mock<DbSet<Plane>>();
            _mockContext.Setup(p => p.Planes).Returns(_mockSet.Object);
            _planeRepository = new PlaneRepository(_mockContext.Object);
        }

        [Fact]
        public async Task GetPlaneById_PlaneExists_ReturnsCorrectPlane()
        {
            // Arrange
            var id = 1;
            var plane = new Plane { Id = id };
            _mockSet.Setup(p => p.FindAsync(id)).ReturnsAsync( plane );

            // Act
            var result = await _planeRepository.GetPlaneById(id);

            // Assert
            Assert.Equal(plane, result);
            _mockSet.Verify(p => p.FindAsync(id), Times.Once());
        }

        [Fact]
        public async Task GetPlaneById_PlaneDoesNotExist_ReturnsNull()
        {
            // Arrane
            var id = 1;
            _mockSet.Setup(p => p.FindAsync(id)).ReturnsAsync((Plane)null);

            // Act
            var result = await _planeRepository.GetPlaneById(id);

            // Assert
            Assert.Null(result);
            _mockSet.Verify(p => p.FindAsync(id), Times.Once());
        }
    }
}
