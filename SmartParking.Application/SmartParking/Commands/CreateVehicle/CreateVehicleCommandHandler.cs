using AutoMapper;
using MediatR;
using SmartParking.Domain.Entities;
using SmartParking.Domain.Interfaces;

namespace SmartParking.Application.SmartParking.Commands.CreateVehicle;

public class CreateVehicleCommandHandler: IRequestHandler<CreateVehicleCommand>
{
    private readonly IMapper _mapper;
    private readonly IVehicleRepositories _vehicleRepositories;

    public CreateVehicleCommandHandler(IMapper mapper, IVehicleRepositories vehicleRepositories)
    {
        _mapper = mapper;
        _vehicleRepositories = vehicleRepositories;
    }
    
    public async Task Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
    {
        var vehicle= _mapper.Map<CreateVehicleCommand, Vehicle>(request);
        await _vehicleRepositories.CreateVehicleAsync(vehicle);
    }
}