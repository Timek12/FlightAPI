using Dapper;
using FlightAPI.Data;
using FlightAPI.Models;
using FlightAPI.Models.DTOs;
using FlightAPI.Repositories.Interfaces;

namespace FlightAPI.Repositories.Implementations
{
    public class PlaneDapperRepository(IDapperContext dapperContext) : IPlaneDapperRepository
    {
        private readonly IDapperContext _dapperContext = dapperContext;

        public Task<IEnumerable<PlaneDTO>> GetAll()
        {
            using var connection = _dapperContext.CreateConnection();

            var query = "SELECT * FROM Planes";

            var planes = connection.QueryAsync<PlaneDTO>(query);

            return planes;
        }

        public Task<Plane?> GetPlaneById(int id)
        {
            using var connection = _dapperContext.CreateConnection();

            var query = "SELECT * FROM Planes p WHERE p.Id = @Id";

            var plane = connection.QueryFirstOrDefaultAsync<Plane>(query, new { Id = id });

            return plane;
        }

        public Task<PlaneDTO?> GetPlaneDTOById(int id)
        {
            using var connection = _dapperContext.CreateConnection();

            var query = "SELECT * FROM Planes p WHERE p.Id = @Id";

            var plane = connection.QueryFirstOrDefaultAsync<PlaneDTO>(query, new { Id = id });

            return plane;

        }
    }
}
