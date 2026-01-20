using AutoMapper;
using MediatR;
using SmartParking.Application.Dtos;
using SmartParking.Application.Interfaces;
using SmartParking.Domain.Interfaces;

namespace SmartParking.Application.SmartParking.Commands.LoginCommand;

public class LoginCommandHandler: IRequestHandler<LoginCommand, AuthResponseDto>
{
    private readonly IUserRepositories _userRepository;
    private readonly IJwtAuth _jwtService;
    private readonly IMapper _mapper;
    
    public LoginCommandHandler(IUserRepositories userRepository, 
        IJwtAuth jwtService,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _mapper = mapper;
    }
    
    public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByUsername(request.Username);
        
        if (user == null)
        {
            throw new Exception("Nieprawidłowa nazwa użytkownika lub hasło");
        }
        
        var isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        
        if (!isPasswordValid)
        {
            throw new Exception("Nieprawidłowa nazwa użytkownika lub hasło");
        }
        
        var userDto = _mapper.Map<UserDto>(user);
        
        var token = _jwtService.GenerateToken(userDto);
        
        return new AuthResponseDto
        {
            Token = token,
            User = userDto
        };
    }
}