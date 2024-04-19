namespace FlightAPI.Exceptions
{
    /// <summary>
    /// Exception that is thrown when the creation of a new user fails.
    /// </summary>
    public class FailedToCreateUserException : Exception
    {
        public FailedToCreateUserException() : base("Failed to create a new user") { }
    }
}
