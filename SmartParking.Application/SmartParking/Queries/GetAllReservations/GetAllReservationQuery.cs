using MediatR;
using SmartParking.Application.Dtos;

namespace SmartParking.Application.SmartParking.Queries.GetAllReservations;

public class GetAllReservationQuery: IRequest<IEnumerable<GetReservationDto>>
{
    
}