namespace FlightAPI.Exceptions
{
    /// <summary>
    /// Exception that is thrown when the creation of a new user fails.
    /// </summary>
    public class FailedToCreateUserException(string message) : Exception(message)
    {
    }
}
