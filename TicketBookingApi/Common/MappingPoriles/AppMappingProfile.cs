using AutoMapper;
using TicketBookingApi.Domain;
using TicketBookingApi.Features.Trips;

namespace TicketBookingApi.Common.MappingPoriles
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<Trip, TripDto>();
        }
    }
}