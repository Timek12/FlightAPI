using FlightAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FlightAPI.Utility.Enums;

namespace FlightAPI.Data
{
    public class ApplicationDbContext(DbContextOptions options) : IdentityDbContext<ApplicationUser>(options), IApplicationDbContext
    {
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Plane> Planes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Plane>().HasData(
                new Plane
                {
                    Id = 1,
                    Model = "A380",
                    Type = "Airbus",
                    Manufacturer = "Airbus",
                    Capacity = 853,
                    CruiseSpeed = 903,
                    Range = 15700
                },
                new Plane
                {
                    Id = 2,
                    Model = "747",
                    Type = "Boeing",
                    Manufacturer = "Boeing",
                    Capacity = 660,
                    CruiseSpeed = 920,
                    Range = 13450
                },
                new Plane
                {
                    Id = 3,
                    Model = "E195",
                    Type = "Embraer",
                    Manufacturer = "Embraer",
                    Capacity = 132,
                    CruiseSpeed = 870,
                    Range = 3700
                },
                new Plane
                {
                    Id = 4,
                    Model = "CRJ1000",
                    Type = "Bombardier",
                    Manufacturer = "Bombardier",
                    Capacity = 104,
                    CruiseSpeed = 829,
                    Range = 2752
                }
            );

            builder.Entity<Flight>().HasData(
                new Flight
                {
                    Id = 1,
                    FlightNumber = "LO280",
                    DepartureDate = new DateTime(2024, 10, 15, 12, 30, 0),
                    DepartureLocation = "Warsaw",
                    ArrivalLocation = "New York",
                    PlaneId = 2
                },
                new Flight
                {
                    Id = 2,
                    FlightNumber = "LO120",
                    DepartureDate = new DateTime(2024, 10, 15, 12, 30, 0),
                    DepartureLocation = "Warsaw",
                    ArrivalLocation = "Tokio",
                    PlaneId = 3 
                },
                new Flight
                {
                    Id = 3,
                    FlightNumber = "LO330",
                    DepartureDate = new DateTime(2024, 10, 15, 12, 30, 0),
                    DepartureLocation = "Warsaw",
                    ArrivalLocation = "Phuket",
                    PlaneId = 1
                }
            );
        }
    }
}
