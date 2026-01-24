using AutoMapper;
using SmartParking.Application.Dtos;
using SmartParking.Application.SmartParking.Commands.CreateParkingZone;
using SmartParking.Application.SmartParking.Commands.LoginCommand;
using SmartParking.Application.SmartParking.Commands.RegisterCommand;
using SmartParking.Application.SmartParking.Commands.UpdateParkingZone;
using SmartParking.Application.SmartParking.Queries.GetAllParkingZones;
using SmartParking.Application.SmartParking.Queries.GetParkingZone;
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
        //CreateMap<GetParkingZoneDto, ParkingZone>().ReverseMap();
        
    }
}