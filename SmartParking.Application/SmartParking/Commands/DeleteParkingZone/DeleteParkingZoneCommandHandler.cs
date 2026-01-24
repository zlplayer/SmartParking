using MediatR;
using SmartParking.Domain.Interfaces;

namespace SmartParking.Application.SmartParking.Commands.DeleteParkingZone;

public class DeleteParkingZoneCommandHandler : IRequestHandler<DeleteParkingZoneCommand>
{
    private readonly IParkingZoneRepositories _parkingZoneRepositories;

    public DeleteParkingZoneCommandHandler(IParkingZoneRepositories parkingZoneRepositories)
    {
        _parkingZoneRepositories = parkingZoneRepositories;
    }
    public async Task Handle(DeleteParkingZoneCommand request, CancellationToken cancellationToken)
    {
        var parkingZone= await _parkingZoneRepositories.GetParkingZoneAsync(request.Id);
        if (parkingZone == null) throw new KeyNotFoundException();
        await _parkingZoneRepositories.DeleteParkingZoneAsync(parkingZone);
    }
}