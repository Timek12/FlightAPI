using System.ComponentModel.DataAnnotations;

namespace FlightAPI.Models.DTOs
{
    public record CreatePlaneDTO
    {
        public string Model { get; set; }
        public string Type { get; set; }
        public string Manufacturer { get; set; }
        public int Capacity { get; set; }
        public double CruiseSpeed { get; set; }
        public double Range { get; set; }
    }
}
