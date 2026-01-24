using AutoMapper;
using MediatR;
using SmartParking.Application.Dtos;
using SmartParking.Domain.Interfaces;

namespace SmartParking.Application.SmartParking.Queries.GetParkingZone;

public class GetParkingZoneQueryHandler: IRequestHandler<GetParkingZoneQuery, GetParkingZoneDto>
{
    private readonly IMapper _mapper;
    private readonly IParkingZoneRepositories _parkingZoneRepositories;

    public GetParkingZoneQueryHandler(IMapper mapper, IParkingZoneRepositories parkingZoneRepositories)
    {
        _mapper = mapper;
        _parkingZoneRepositories = parkingZoneRepositories;
    }
    public async Task<GetParkingZoneDto> Handle(GetParkingZoneQuery request, CancellationToken cancellationToken)
    {
        var parkingZone = await _parkingZoneRepositories.GetParkingZoneAsync(request.Id);
        if (parkingZone == null) throw new KeyNotFoundException();
        return _mapper.Map<GetParkingZoneDto>(parkingZone);
    }
}