using MediatR;
using SmartParking.Domain.Interfaces;

namespace SmartParking.Application.SmartParking.Commands.DeleteParkingSpace;

public class DeleteParkingSpaceCommandHandler: IRequestHandler<DeleteParkingSpaceCommand>
{
    private readonly IParkingSpaceRepositories _parkingSpaceRepositories;

    public DeleteParkingSpaceCommandHandler(IParkingSpaceRepositories parkingSpaceRepositories)
    {
        _parkingSpaceRepositories = parkingSpaceRepositories;
    }
    public async Task Handle(DeleteParkingSpaceCommand request, CancellationToken cancellationToken)
    {
       var parkingSpace = await _parkingSpaceRepositories.GetParkingSpaceByIdAsync(request.Id);
       if (parkingSpace == null) throw new KeyNotFoundException();
       await _parkingSpaceRepositories.DeleteParkingSpaceAsync(parkingSpace);
    }
}