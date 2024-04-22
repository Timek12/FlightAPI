namespace FlightAPI.Models.DTOs
{
    public class LoginResponseDTO
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expiration {  get; set; }
    }
}
