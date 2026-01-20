using MediatR;
using SmartParking.Application.Dtos;

namespace SmartParking.Application.SmartParking.Queries.GetUser;

public class GetUserQuery: IRequest<UserDto>
{
    public Guid UserId { get; set; }
}