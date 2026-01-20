using SmartParking.Application.Dtos;

namespace SmartParking.Application.Interfaces;

public interface IJwtAuth
{
    string GenerateToken(UserDto user);

}