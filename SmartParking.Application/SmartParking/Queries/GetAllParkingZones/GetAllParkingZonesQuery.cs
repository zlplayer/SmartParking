using MediatR;
using SmartParking.Application.Dtos;

namespace SmartParking.Application.SmartParking.Queries.GetAllParkingZones;

public class GetAllParkingZonesQuery:IRequest<IEnumerable<GetParkingZoneDto>>
{
    
}