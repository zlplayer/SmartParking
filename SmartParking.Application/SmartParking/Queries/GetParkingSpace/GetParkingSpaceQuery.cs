using MediatR;
using SmartParking.Application.Dtos;

namespace SmartParking.Application.SmartParking.Queries.GetParkingSpace;

public class GetParkingSpaceQuery: IRequest<GetParkingSpaceDto>
{
    public Guid Id { get; set; }
}