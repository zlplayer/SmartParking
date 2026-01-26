using MediatR;
using SmartParking.Application.Dtos;

namespace SmartParking.Application.SmartParking.Queries.GetReservationById;

public class GetReservationByIdQuery: IRequest<GetReservationDto>
{
    public Guid Id { get; set; }
}