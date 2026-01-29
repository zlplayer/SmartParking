using MediatR;
using SmartParking.Application.Dtos;

namespace SmartParking.Application.SmartParking.Queries.GetAllAvailableParkingSpace;

public class GetAllAvailableParkingSpaceQuery: IRequest<IEnumerable<GetParkingSpaceDto>>
{
    
}