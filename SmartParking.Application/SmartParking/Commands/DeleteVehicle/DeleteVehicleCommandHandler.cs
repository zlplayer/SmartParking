using MediatR;
using SmartParking.Domain.Interfaces;

namespace SmartParking.Application.SmartParking.Commands.DeleteVehicle;

public class DeleteVehicleCommandHandler: IRequestHandler<DeleteVehicleCommand>
{
    private readonly IVehicleRepositories _vehicleRepositories;

    public DeleteVehicleCommandHandler(IVehicleRepositories  vehicleRepositories)
    {
        _vehicleRepositories = vehicleRepositories;
    }

    public async Task Handle(DeleteVehicleCommand request, CancellationToken cancellationToken)
    {
        var vehicle = await _vehicleRepositories.GetVehicleAsync(request.Id);
        if (vehicle == null) throw new KeyNotFoundException();
        await _vehicleRepositories.DeleteVehicleAsync(vehicle);
    }
}