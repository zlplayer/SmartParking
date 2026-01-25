using AutoMapper;
using MediatR;
using SmartParking.Domain.Interfaces;

namespace SmartParking.Application.SmartParking.Commands.UpdateVehicle;

public class UpdateVehicleCommandHandler: IRequestHandler<UpdateVehicleCommand>
{
    private readonly IMapper _mapper;
    private readonly IVehicleRepositories _vehicleRepositories;

    public UpdateVehicleCommandHandler(IMapper mapper, IVehicleRepositories  vehicleRepositories)
    {
        _mapper = mapper;
        _vehicleRepositories = vehicleRepositories;
    }
    public async Task Handle(UpdateVehicleCommand request, CancellationToken cancellationToken)
    {
        var vehicle = await _vehicleRepositories.GetVehicleAsync(request.Id);
        if (vehicle == null) throw new KeyNotFoundException();
        _mapper.Map(request, vehicle);
        await _vehicleRepositories.UpdateVehicleAsync(vehicle);
    }
}