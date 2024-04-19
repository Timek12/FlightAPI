namespace FlightAPI.Exceptions
{
    /// <summary>
    /// Exception that is thrown when FlightId is less than or equal 0.
    /// </summary>
    public class InvalidFlightIdException : Exception
    {
        public InvalidFlightIdException() : base("FlightId should be greater than 0") { }
    }
}
