using AutoMapper;
using TicketBookingApi.Domain;
using TicketBookingApi.Features.Auth.Register;
using TicketBookingApi.Features.Tickets;
using TicketBookingApi.Features.Trips;

namespace TicketBookingApi.Common.MappingPoriles
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<Trip, TripDto>();

            CreateMap<Ticket, TicketDto>()
                .ForMember(dest => dest.From, opt => opt.MapFrom(src => src.Trip.From))
                .ForMember(dest => dest.To, opt => opt.MapFrom(src => src.Trip.To));

            CreateMap<RegisterCommand, RegisterDto>();
            
            CreateMap<RegisterDto, User>();
        }
    }
}