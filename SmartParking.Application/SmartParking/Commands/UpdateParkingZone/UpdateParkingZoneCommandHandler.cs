using AutoMapper;
using MediatR;
using SmartParking.Domain.Entities;
using SmartParking.Domain.Interfaces;

namespace SmartParking.Application.SmartParking.Commands.UpdateParkingZone;

public class UpdateParkingZoneCommandHandler: IRequestHandler<UpdateParkingZoneCommand>
{
    private readonly IMapper _mapper;
    private readonly IParkingZoneRepositories _parkingZoneRepositories;

    public UpdateParkingZoneCommandHandler(IMapper mapper, IParkingZoneRepositories parkingZoneRepositories)
    {
        _mapper=mapper;
        _parkingZoneRepositories = parkingZoneRepositories;
    }
    public async Task Handle(UpdateParkingZoneCommand request, CancellationToken cancellationToken)
    {
        var parkingZone = await _parkingZoneRepositories.GetParkingZoneAsync(request.Id);
        if (parkingZone == null) throw new KeyNotFoundException();
        _mapper.Map(request, parkingZone);
        await _parkingZoneRepositories.UpdateParkingZoneAsync(parkingZone);
    }
}