namespace FlightAPI.Models
{
    public class Plane
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string Type { get; set; }
        public string Manufacturer { get; set; }
        public int Capacity { get; set; }
        public double CruiseSpeed { get; set; }
        public double Range { get; set; }
    }
}
