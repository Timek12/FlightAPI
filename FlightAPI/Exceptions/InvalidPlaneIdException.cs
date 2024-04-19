namespace FlightAPI.Exceptions
{
    /// <summary>
    /// Exception that is thrown when the PlaneId is less than or equal 0.
    /// </summary>
    public class InvalidPlaneIdException : Exception
    {
        public InvalidPlaneIdException() : base("PlaneId should be greater than 0") { }
    }
}
