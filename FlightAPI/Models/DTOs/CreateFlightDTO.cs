namespace FlightAPI.Models.DTOs
{
    public record CreateFlightDTO
    {
        public string FlightNumber { get; set; }
        public DateTime? DepartureDate { get; set; }
        public string ArrivalLocation { get; set; }
        public string DepartureLocation { get; set; }
        public int PlaneId { get; set; }
    }
}
