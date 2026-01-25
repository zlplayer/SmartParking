using AutoMapper;
using MediatR;
using SmartParking.Application.Dtos;
using SmartParking.Domain.Interfaces;

namespace SmartParking.Application.SmartParking.Queries.GetAllParkingSpace;

public class GetAllParkingSpaceQueryHandler: IRequestHandler<GetAllParkingSpaceQuery, IEnumerable<GetParkingSpaceDto>>
{
    private readonly IMapper _mapper;
    private readonly IParkingSpaceRepositories _parkingSpaceRepositories;

    public GetAllParkingSpaceQueryHandler(IMapper mapper, IParkingSpaceRepositories parkingSpaceRepositories)
    {
        _mapper = mapper;
        _parkingSpaceRepositories = parkingSpaceRepositories;
    }
    public async Task<IEnumerable<GetParkingSpaceDto>> Handle(GetAllParkingSpaceQuery request, CancellationToken cancellationToken)
    {
        var parkingSpace= await _parkingSpaceRepositories.GetAllParkingSpaceAsync();
        return _mapper.Map<IEnumerable<GetParkingSpaceDto>>(parkingSpace);
    }
}