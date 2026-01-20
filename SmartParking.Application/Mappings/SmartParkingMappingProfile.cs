using AutoMapper;
using SmartParking.Application.Dtos;
using SmartParking.Application.SmartParking.Commands.LoginCommand;
using SmartParking.Application.SmartParking.Commands.RegisterCommand;
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
    }
}