namespace FlightAPI.Exceptions
{
    /// <summary>
    /// Exception that is thrown when the plane with a given PlaneId does not exist in database.
    /// </summary>
    public class PlaneNotFoundException : Exception
    {
        public PlaneNotFoundException() : base("Plane with a given id does not exist") { }
    }
}
