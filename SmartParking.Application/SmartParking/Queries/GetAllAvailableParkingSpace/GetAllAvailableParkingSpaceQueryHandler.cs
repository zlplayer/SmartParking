using AutoMapper;
using MediatR;
using SmartParking.Application.Dtos;
using SmartParking.Domain.Interfaces;

namespace SmartParking.Application.SmartParking.Queries.GetAllAvailableParkingSpace;

public class GetAllAvailableParkingSpaceQueryHandler: IRequestHandler<GetAllAvailableParkingSpaceQuery, IEnumerable<GetParkingSpaceDto>>
{
    private readonly IMapper _mapper;
    private readonly IParkingSpaceRepositories _parkingSpaceRepositories;

    public GetAllAvailableParkingSpaceQueryHandler(IMapper mapper, IParkingSpaceRepositories parkingSpaceRepositories)
    {
        _mapper = mapper;
        _parkingSpaceRepositories = parkingSpaceRepositories;
    }
    public async Task<IEnumerable<GetParkingSpaceDto>> Handle(GetAllAvailableParkingSpaceQuery request, CancellationToken cancellationToken)
    {
        var parkingSpace = await _parkingSpaceRepositories.GetAllAvailableParkingSpaceAsync();
        return _mapper.Map<IEnumerable<GetParkingSpaceDto>>(parkingSpace);
    }
}