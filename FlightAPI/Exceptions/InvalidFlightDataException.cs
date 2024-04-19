namespace FlightAPI.Exceptions
{
    /// <summary>
    /// Exception that is thrown when the mismatch between URL id and flightDTO id occurs.
    /// </summary>
    public class InvalidFlightDataException : Exception
    {
        public InvalidFlightDataException() : base("Mismatch between URL id and flightDTO id") { }
    }
}
