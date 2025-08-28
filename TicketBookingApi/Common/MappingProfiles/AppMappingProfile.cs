using AutoMapper;
using TicketBookingApi.Domain;
using TicketBookingApi.Features.Auth.Register;
using TicketBookingApi.Features.Tickets;
using TicketBookingApi.Features.Trips;
using TicketBookingApi.Features.Trips.CreateTrip;
using TicketBookingApi.Features.Users;

namespace TicketBookingApi.Common.MappingProfiles
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

            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Roles, opt => opt.Ignore());

            CreateMap<CreateTripCommand, Trip>();
        }
    }
}