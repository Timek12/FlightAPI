using FlightAPI.Utility.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightAPI.Models
{
    public class Flight
    {
        public int Id { get; set; }
        public string FlightNumber {  get; set; }
        public DateTime? DepartureDate { get; set; }
        public string ArrivalLocation { get; set; }
        public string DepartureLocation { get; set; }
        public PlaneType PlaneType { get; set; }

        [ForeignKey(nameof(Plane))]
        public int PlaneId { get; set; }
        public Plane Plane { get; set; }
    }
}
