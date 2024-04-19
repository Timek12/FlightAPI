namespace FlightAPI.Exceptions
{
    /// <summary>
    /// Exception that is thrown when the given password is invalid.
    /// </summary>
    public class InvalidPasswordException : Exception
    {
        public InvalidPasswordException() : base("Invalid password") { }
    }
}
