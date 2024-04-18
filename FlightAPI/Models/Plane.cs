using System.ComponentModel.DataAnnotations;

namespace FlightAPI.Models
{
    public class Plane
    {
        public int Id { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public string Type { get; set; }
        public string Manufacturer { get; set; }
        public int Capacity { get; set; }
        public double CruiseSpeed { get; set; }
        public double Range { get; set; }
    }
}
