using AutoMapper;
using MediatR;
using SmartParking.Domain.Entities;
using SmartParking.Domain.Interfaces;

namespace SmartParking.Application.SmartParking.Commands.RegisterCommand;

public class RegisterCommandHandler:  IRequestHandler<RegisterCommand>
{
    private readonly IUserRepositories _userRepository;
    private readonly IMapper _mapper;
    
    public RegisterCommandHandler(IUserRepositories userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }


    public async Task Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetUserByEmail(request.Email);
        if (existingUser != null)
        {
            throw new Exception("Użytkownik z takim emailem już istnieje");
        }

        var existingUsername = await _userRepository.GetUserByUsername(request.Username);
        if (existingUsername != null)
        {
            throw new Exception("Użytkownik z taką nazwą już istnieje");
        }
        
        request.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
        
        var user = _mapper.Map<User>(request);
        
        await _userRepository.CreateUser(user); 
    }
}