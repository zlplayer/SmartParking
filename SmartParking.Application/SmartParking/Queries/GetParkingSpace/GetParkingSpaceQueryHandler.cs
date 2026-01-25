using AutoMapper;
using MediatR;
using SmartParking.Application.Dtos;
using SmartParking.Domain.Interfaces;

namespace SmartParking.Application.SmartParking.Queries.GetParkingSpace;

public class GetParkingSpaceQueryHandler: IRequestHandler<GetParkingSpaceQuery, GetParkingSpaceDto>
{
    private readonly IMapper _mapper;
    private readonly IParkingSpaceRepositories _parkingSpaceRepositories;

    public GetParkingSpaceQueryHandler(IMapper mapper, IParkingSpaceRepositories parkingSpaceRepositories)
    {
        _mapper = mapper;
        _parkingSpaceRepositories=parkingSpaceRepositories;
    }
    public async Task<GetParkingSpaceDto> Handle(GetParkingSpaceQuery request, CancellationToken cancellationToken)
    {
        var parkingSpace = await _parkingSpaceRepositories.GetParkingSpaceByIdAsync(request.Id);
        return _mapper.Map<GetParkingSpaceDto>(parkingSpace);
    }
}