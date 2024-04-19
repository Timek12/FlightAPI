namespace FlightAPI.Exceptions
{
    /// <summary>
    /// Exception that is thrown user with a given email does not exist in database.
    /// </summary>
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException() : base("User with given email does not exist") { }
    }
}
