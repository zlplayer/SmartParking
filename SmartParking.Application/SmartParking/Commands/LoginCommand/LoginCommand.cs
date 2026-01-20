using MediatR;
using SmartParking.Application.Dtos;

namespace SmartParking.Application.SmartParking.Commands.LoginCommand;

public class LoginCommand: LoginDto, IRequest<AuthResponseDto>
{
    
}