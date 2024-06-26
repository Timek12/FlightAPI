﻿namespace FlightAPI.Models.DTOs
{
    public record FlightDTO
    {
        public int Id { get; set; }
        public string FlightNumber { get; set; }
        public DateTime? DepartureDate { get; set; }
        public string ArrivalLocation { get; set; }
        public string DepartureLocation { get; set; }
        public PlaneDTO Plane { get; set; }
    }
}
