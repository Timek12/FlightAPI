namespace FlightAPI.Exceptions
{
    /// <summary>
    /// Exception is thrown if user already exist in database.
    /// </summary>
    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException() : base("User already exists") {}
    }

}
