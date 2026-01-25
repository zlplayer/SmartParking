using MediatR;
using SmartParking.Application.Dtos;

namespace SmartParking.Application.SmartParking.Queries.GetAllVehicleByUserId;

public class GetAllVehicleByUserIdQuery: IRequest<IEnumerable<GetVehicleDto>>
{
    public Guid UserId { get; set; }
}