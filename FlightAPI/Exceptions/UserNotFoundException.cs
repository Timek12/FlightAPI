namespace FlightAPI.Exceptions
{
    /// <summary>
    /// Exception that is thrown when user does not exist in database.
    /// </summary>
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException() : base("User not found") { }
    }
}
