using FlightAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FlightAPI.Utility.Enums;

namespace FlightAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Flight> Flights { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Flight>().HasData(
                new Flight
                {
                    Id = 1,
                    FlightNumber = "LO280",
                    DepartureDate = new DateTime(2024, 10, 15, 12, 30, 0),
                    DepartureLocation = "Warsaw",
                    ArrivalLocation = "New York",
                    PlaneType = PlaneType.Boeing
                },
                new Flight
                {
                    Id = 2,
                    FlightNumber = "LO120",
                    DepartureDate = new DateTime(2024, 10, 15, 12, 30, 0),
                    DepartureLocation = "Warsaw",
                    ArrivalLocation = "Tokio",
                    PlaneType = PlaneType.Embraer
                },
                new Flight
                {
                    Id = 3,
                    FlightNumber = "LO330",
                    DepartureDate = new DateTime(2024, 10, 15, 12, 30, 0),
                    DepartureLocation = "Warsaw",
                    ArrivalLocation = "Phuket",
                    PlaneType = PlaneType.Airbus
                }
            );
        }
    }
}
