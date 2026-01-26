using AutoMapper;
using SmartParking.Application.Dtos;
using SmartParking.Application.SmartParking.Commands.CancelReservation;
using SmartParking.Application.SmartParking.Commands.CreateParkingSpace;
using SmartParking.Application.SmartParking.Commands.CreateParkingZone;
using SmartParking.Application.SmartParking.Commands.CreateReservation;
using SmartParking.Application.SmartParking.Commands.CreateVehicle;
using SmartParking.Application.SmartParking.Commands.LoginCommand;
using SmartParking.Application.SmartParking.Commands.RegisterCommand;
using SmartParking.Application.SmartParking.Commands.UpdateParkingSpace;
using SmartParking.Application.SmartParking.Commands.UpdateParkingZone;
using SmartParking.Application.SmartParking.Commands.UpdateReservation;
using SmartParking.Application.SmartParking.Commands.UpdateVehicle;
using SmartParking.Application.SmartParking.Queries.GetAllParkingSpace;
using SmartParking.Application.SmartParking.Queries.GetAllParkingZones;
using SmartParking.Application.SmartParking.Queries.GetAllReservations;
using SmartParking.Application.SmartParking.Queries.GetAllVehicleByUserId;
using SmartParking.Application.SmartParking.Queries.GetParkingSpace;
using SmartParking.Application.SmartParking.Queries.GetParkingZone;
using SmartParking.Application.SmartParking.Queries.GetVehicle;
using SmartParking.Domain.Entities;

namespace SmartParking.Application.Mappings;

public class SmartParkingMappingProfile:Profile
{
    public SmartParkingMappingProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name)).ReverseMap();
        
        CreateMap<RegisterDto, RegisterCommand>();
        CreateMap<LoginDto, LoginCommand>();

        CreateMap<RegisterCommand, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));
        
        CreateMap<CreateParkingZoneCommand, ParkingZone>().ReverseMap();
        CreateMap<UpdateParkingZoneCommand, ParkingZone>().ReverseMap();
        CreateMap<GetAllParkingZonesQuery, ParkingZone>().ReverseMap();
        CreateMap<GetParkingZoneQuery, ParkingZone>().ReverseMap();
        CreateMap<GetParkingZoneDto, ParkingZone>().ReverseMap();
        
        CreateMap<CreateParkingSpaceCommand, ParkingSpace>().ReverseMap();
        CreateMap<UpdateParkingSpaceCommand, ParkingSpace>().ReverseMap();
        CreateMap<GetAllParkingSpaceQuery, ParkingSpace>().ReverseMap();
        CreateMap<GetParkingSpaceQuery, ParkingSpace>().ReverseMap();
        CreateMap<GetParkingSpaceDto, ParkingSpace>().ReverseMap();
        
        CreateMap<CreateVehicleCommand, Vehicle>().ReverseMap();
        CreateMap<UpdateVehicleCommand, Vehicle>().ReverseMap();
        CreateMap<GetAllVehicleByUserIdQuery, Vehicle>().ReverseMap();
        CreateMap<GetVehicleQuery, Vehicle>().ReverseMap();
        CreateMap<GetVehicleDto, Vehicle>().ReverseMap();
        
        CreateMap<GetReservationDto, Reservation>().ReverseMap();
        CreateMap<CreateReservationCommand, Reservation>().ReverseMap();
        CreateMap<UpdateReservationCommand, Reservation>().ReverseMap();
        CreateMap<CancelReservationCommand, Reservation>().ReverseMap();
    }
}