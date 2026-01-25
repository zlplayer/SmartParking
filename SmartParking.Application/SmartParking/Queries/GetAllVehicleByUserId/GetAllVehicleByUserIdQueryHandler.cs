using AutoMapper;
using MediatR;
using SmartParking.Application.Dtos;
using SmartParking.Domain.Interfaces;

namespace SmartParking.Application.SmartParking.Queries.GetAllVehicleByUserId;

public class GetAllVehicleByUserIdQueryHandler: IRequestHandler<GetAllVehicleByUserIdQuery, IEnumerable<GetVehicleDto>>
{
    private readonly IMapper _mapper;
    private readonly IVehicleRepositories _vehicleRepositories;

    public GetAllVehicleByUserIdQueryHandler(IMapper mapper,IVehicleRepositories  vehicleRepositories)
    {
        _mapper = mapper;
        _vehicleRepositories = vehicleRepositories;
    }
    public async Task<IEnumerable<GetVehicleDto>> Handle(GetAllVehicleByUserIdQuery request, CancellationToken cancellationToken)
    {
        var vehicleByUSerId = await _vehicleRepositories.GetVehiclesByUserIdAsync(request.UserId);
        return _mapper.Map<IEnumerable<GetVehicleDto>>(vehicleByUSerId);
    }
}