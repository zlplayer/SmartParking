using AutoMapper;
using MediatR;
using SmartParking.Domain.Entities;
using SmartParking.Domain.Interfaces;

namespace SmartParking.Application.SmartParking.Commands.CreateParkingZone;

public class CreateParkingZoneCommandHandler: IRequestHandler<CreateParkingZoneCommand>
{
    private readonly IMapper _mapper;
    private readonly IParkingZoneRepositories _parkingZoneRepositories;

    public CreateParkingZoneCommandHandler(IMapper mapper, IParkingZoneRepositories parkingZoneRepositories)
    {
        _mapper = mapper;
        _parkingZoneRepositories = parkingZoneRepositories;
    }
    public async Task Handle(CreateParkingZoneCommand request, CancellationToken cancellationToken)
    {
        var parkingZone = _mapper.Map<CreateParkingZoneCommand, ParkingZone>(request);
        await _parkingZoneRepositories.CreateParkingZoneAsync(parkingZone);
    }
}