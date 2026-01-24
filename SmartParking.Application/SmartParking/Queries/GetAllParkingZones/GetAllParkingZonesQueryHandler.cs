using AutoMapper;
using MediatR;
using SmartParking.Application.Dtos;
using SmartParking.Domain.Interfaces;

namespace SmartParking.Application.SmartParking.Queries.GetAllParkingZones;

public class GetAllParkingZonesQueryHandler: IRequestHandler<GetAllParkingZonesQuery, IEnumerable<GetParkingZoneDto>>
{
    private readonly IMapper _mapper;
    private readonly IParkingZoneRepositories _parkingZoneRepositories;

    public GetAllParkingZonesQueryHandler(IMapper mapper, IParkingZoneRepositories parkingZoneRepositories)
    {
        _mapper = mapper;
        _parkingZoneRepositories = parkingZoneRepositories;
    }
    public async Task<IEnumerable<GetParkingZoneDto>> Handle(GetAllParkingZonesQuery request, CancellationToken cancellationToken)
    {
        var allParkingZones= await _parkingZoneRepositories.GetAllParkingzonesAsync();
        return _mapper.Map<IEnumerable<GetParkingZoneDto>>(allParkingZones);
    }
}