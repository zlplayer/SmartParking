using MediatR;

namespace SmartParking.Application.SmartParking.Commands.DeleteVehicle;

public class DeleteVehicleCommand: IRequest
{
    public Guid Id { get; set; }
}