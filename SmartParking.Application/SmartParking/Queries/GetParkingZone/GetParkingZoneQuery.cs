using MediatR;
using SmartParking.Application.Dtos;

namespace SmartParking.Application.SmartParking.Queries.GetParkingZone;

public class GetParkingZoneQuery: IRequest<GetParkingZoneDto>
{
    public Guid Id { get; set; }
}