using AutoMapper;
using MediatR;
using SmartParking.Application.Dtos;
using SmartParking.Domain.Interfaces;

namespace SmartParking.Application.SmartParking.Queries.GetVehicle;

public class GetVehicleQueryHandler : IRequestHandler<GetVehicleQuery, GetVehicleDto>
{
    private readonly IMapper _mapper;
    private readonly IVehicleRepositories _vehicleRepositories;

    public GetVehicleQueryHandler(IMapper mapper,IVehicleRepositories  vehicleRepositories)
    {
        _mapper = mapper;
        _vehicleRepositories = vehicleRepositories;
    }
    public async Task<GetVehicleDto> Handle(GetVehicleQuery request, CancellationToken cancellationToken)
    {
        var vehicleDetails = await _vehicleRepositories.GetVehicleAsync(request.VehicleId);
        return _mapper.Map<GetVehicleDto>(vehicleDetails);
    }
}