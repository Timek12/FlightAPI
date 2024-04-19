namespace FlightAPI.Exceptions
{
    /// <summary>
    /// Exception that is thrown when the flight with given id does not exist in database.
    /// </summary>
    public class FlightNotFoundException : Exception
    {
        public FlightNotFoundException() : base("Flight with a given id does not exist") { }
    }
}
