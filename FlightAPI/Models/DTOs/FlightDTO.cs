using FlightAPI.Utility.Enums;

namespace FlightAPI.Models.DTOs
{
    public class FlightDTO
    {
        public int Id { get; set; }
        public string FlightNumber { get; set; }
        public DateTime? DepartureDate { get; set; }
        public string ArrivalLocation { get; set; }
        public string DepartureLocation { get; set; }
        public string PlaneType { get; set; }
    }
}
