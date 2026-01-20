using AutoMapper;
using MediatR;
using SmartParking.Application.Dtos;
using SmartParking.Domain.Interfaces;

namespace SmartParking.Application.SmartParking.Queries.GetUser;

public class GetUserQueryHandler: IRequestHandler<GetUserQuery, UserDto>
{
    private readonly IUserRepositories _userRepository;
    private readonly IMapper _mapper;

    public GetUserQueryHandler(IUserRepositories userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;

    }
    public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUser(request.UserId);

        if (user == null)
        {
            throw new Exception("Użytkownik nie znaleziony");
        }

        return _mapper.Map<UserDto>(user);
    }
}