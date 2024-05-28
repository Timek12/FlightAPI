using Microsoft.Data.SqlClient;
using System.Data;

namespace FlightAPI.Data
{
    public class DapperContext(IConfiguration configuration) : IDapperContext
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly string _connectionString = configuration.GetConnectionString("DefaultDbConnection");

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
