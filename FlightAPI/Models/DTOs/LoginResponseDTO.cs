namespace FlightAPI.Models.DTOs
{
    public record LoginResponseDTO
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expiration {  get; set; }
    }
}
