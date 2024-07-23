using FlightAPI.Data;
using FlightAPI.Models.DTOs;
using FlightAPI.Models;
using FlightAPI.Repositories.Interfaces;
using Dapper;

namespace FlightAPI.Repositories.Implementations
{
    public class FlightDapperRepository(IDapperContext dapperContext) : IFlightDapperRepository
    {
        private readonly IDapperContext _dapperContext = dapperContext;

        public async Task<IEnumerable<FlightDTO>> GetAll()
        {
            using var connection = _dapperContext.CreateConnection();

            var query = @"
                SELECT  *
                FROM dbo.Flights f
                INNER JOIN Planes p on f.PlaneId = p.Id
            ";

            var flights = await connection.QueryAsync<FlightDTO, PlaneDTO, FlightDTO>(
                query,
                (flight, plane) =>
                {
                    flight.Plane = plane;
                    return flight;
                },
                splitOn: "PlaneId"
                );

            return flights;
        }

        public async Task<Flight?> GetFlightById(int id)
        {
            using var connection = _dapperContext.CreateConnection();

            var query = "SELECT * FROM dbo.Flights f WHERE f.Id = @Id";

            var flight = await connection.QueryFirstOrDefaultAsync<Flight>(query, new { Id = id });

            return flight;
        }

        public async Task<FlightDTO?> GetFlightDTOById(int id)
        {
            using var connection = _dapperContext.CreateConnection();

            var query = "SELECT * FROM dbo.Flights f WHERE f.Id = @Id";

            var flight = await connection.QueryFirstOrDefaultAsync<FlightDTO>(query, new { Id = id });

            return flight;
        }
    }
}
