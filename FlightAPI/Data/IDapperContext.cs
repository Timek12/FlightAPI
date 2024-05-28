using System.Data;

namespace FlightAPI.Data
{
    public interface IDapperContext
    {
       public IDbConnection CreateConnection();
    }
}
