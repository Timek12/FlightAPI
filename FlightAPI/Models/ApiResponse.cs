using System.ComponentModel;
using System.Net;

namespace FlightAPI.Models
{
    public class ApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = false;
        public List<string> Errors { get; set; } = [];
        public object Result { get; set; }
    }
}
