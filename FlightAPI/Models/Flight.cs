namespace FlightAPI.Models
{
    public class Flight
    {
        public int Id { get; set; }
        public string FlightNumber {  get; set; }
        public DateTime? Departure { get; set; }
        public string Destination { get; set; }
        public string DeparturePoint { get; set; }

    }
}
