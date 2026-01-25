using AutoMapper;
using MediatR;
using SmartParking.Domain.Interfaces;

namespace SmartParking.Application.SmartParking.Commands.UpdateParkingSpace;

public class UpdateParkingSpaceCommandHandler: IRequestHandler<UpdateParkingSpaceCommand>
{
    private readonly IMapper _mapper;
    private readonly IParkingSpaceRepositories _parkingSpaceRepositories;

    public UpdateParkingSpaceCommandHandler(IMapper mapper, IParkingSpaceRepositories parkingSpaceRepositories)
    {
        _mapper = mapper;
        _parkingSpaceRepositories = parkingSpaceRepositories;
    }
    public async Task Handle(UpdateParkingSpaceCommand request, CancellationToken cancellationToken)
    {
        var parkingSpace= await _parkingSpaceRepositories.GetParkingSpaceByIdAsync(request.Id);
        if (parkingSpace == null) throw new KeyNotFoundException();
        _mapper.Map(request, parkingSpace);
        await _parkingSpaceRepositories.UpdateParkingSpaceAsync(parkingSpace);
    }
}