using MediatR;
using SmartParking.Application.Dtos;

namespace SmartParking.Application.SmartParking.Queries.GetVehicle;

public class GetVehicleQuery: IRequest<GetVehicleDto>
{
    public Guid VehicleId { get; set; }
}