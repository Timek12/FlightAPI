﻿namespace FlightAPI.Exceptions
{
    public class InvalidAccessTokenException : Exception
    {
        public InvalidAccessTokenException() : base("Invalid access token") {}
    }
}
