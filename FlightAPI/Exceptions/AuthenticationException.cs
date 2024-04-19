namespace FlightAPI.Exceptions
{
    /// <summary>
    /// Exception that is thrown when the given password is invalid.
    /// </summary>
    public class AuthenticationException : Exception
    {
        public AuthenticationException() : base("Authentication failed") { }
    }
}
