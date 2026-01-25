using MediatR;
using SmartParking.Application.Dtos;

namespace SmartParking.Application.SmartParking.Queries.GetAllParkingSpace;

public class GetAllParkingSpaceQuery: IRequest<IEnumerable<GetParkingSpaceDto>>
{
    
}