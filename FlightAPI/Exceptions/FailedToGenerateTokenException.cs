namespace FlightAPI.Exceptions
{
    /// <summary>
    /// Exception that is thrown when the generation of a JWT token fails.
    /// </summary>
    public class FailedToGenerateTokenException : Exception
    {
        public FailedToGenerateTokenException() : base("Failed to generate a JWT token") { }
    }
}
