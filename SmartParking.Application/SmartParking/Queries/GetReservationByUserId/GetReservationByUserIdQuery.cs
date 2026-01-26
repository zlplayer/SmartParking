using MediatR;
using SmartParking.Application.Dtos;

namespace SmartParking.Application.SmartParking.Queries.GetReservationByUserId;

public class GetReservationByUserIdQuery: IRequest<IEnumerable<GetReservationDto>>
{
    public Guid UserId { get; set; }
}