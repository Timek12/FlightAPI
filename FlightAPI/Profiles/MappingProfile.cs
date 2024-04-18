using AutoMapper;
using FlightAPI.Models;
using FlightAPI.Models.DTOs;

namespace FlightAPI.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Flight, FlightDTO>();
            CreateMap<Plane, PlaneDTO>();
            CreateMap<UpdateFlightDTO, Flight>();
        }
    }
}
