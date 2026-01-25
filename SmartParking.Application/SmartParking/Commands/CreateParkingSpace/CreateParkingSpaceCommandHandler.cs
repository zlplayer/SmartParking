using AutoMapper;
using MediatR;
using SmartParking.Domain.Entities;
using SmartParking.Domain.Interfaces;

namespace SmartParking.Application.SmartParking.Commands.CreateParkingSpace;

public class CreateParkingSpaceCommandHandler: IRequestHandler<CreateParkingSpaceCommand>
{
    private readonly IMapper _mapper;
    private readonly IParkingSpaceRepositories _parkingSpaceRepositories;

    public CreateParkingSpaceCommandHandler(IMapper mapper, IParkingSpaceRepositories parkingSpaceRepositories)
    {
        _mapper = mapper;
        _parkingSpaceRepositories = parkingSpaceRepositories;
    }
    public async Task Handle(CreateParkingSpaceCommand request, CancellationToken cancellationToken)
    {
        var parkingSpace = _mapper.Map<CreateParkingSpaceCommand, ParkingSpace>(request);
        await _parkingSpaceRepositories.CreateParkingSpaceAsync(parkingSpace);
    }
}