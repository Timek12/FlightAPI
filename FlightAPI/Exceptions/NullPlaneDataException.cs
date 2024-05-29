namespace FlightAPI.Exceptions
{
    public class NullPlaneDataException : Exception
    {
        /// <summary>
        /// Exception that is thrown when the plane entity is null.
        /// </summary>
        public NullPlaneDataException() : base("Plane data cannot be null") { }
    }
}
