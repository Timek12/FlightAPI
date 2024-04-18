using FlightAPI.Utility.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightAPI.Models
{
    public class Flight
    {
        public int Id { get; set; }
        [Required]
        public string FlightNumber {  get; set; }
        [Required]
        public DateTime? DepartureDate { get; set; }
        [Required]
        public string ArrivalLocation { get; set; }
        [Required]
        public string DepartureLocation { get; set; }
        [Required]

        [ForeignKey(nameof(Plane))]
        public int PlaneId { get; set; }
        public Plane Plane { get; set; }
    }
}
