namespace FlightAPI.Exceptions
{
    public class NullFlightDataException : Exception
    {
        /// <summary>
        /// Exception that is thrown when the flight entity is null.
        /// </summary>
        public NullFlightDataException() : base("Flight data cannot be null") { }
    }
}
