using AutoMapper;
using MediatR;
using SmartParking.Domain.Interfaces;

namespace SmartParking.Application.SmartParking.Commands.CancelReservation;

public class CancelReservationCommandHandler: IRequestHandler<CancelReservationCommand>
{
    private readonly IMapper _mapper;
    private readonly IReservationRepositories _reservationRepositories;
    private readonly IParkingSpaceRepositories _parkingSpaceRepositories;

    public CancelReservationCommandHandler(IMapper mapper, IReservationRepositories reservationRepositories, IParkingSpaceRepositories parkingSpaceRepositories)
    {
        _mapper = mapper;
        _reservationRepositories = reservationRepositories;
        _parkingSpaceRepositories = parkingSpaceRepositories;

    }
    public async Task Handle(CancelReservationCommand request, CancellationToken cancellationToken)
    {
        var reservation =await _reservationRepositories.GetReservationByIdAsync(request.Id);
        if (reservation == null) throw new KeyNotFoundException();
        _mapper.Map(request, reservation);
        await _reservationRepositories.UpdateReservationAsync(reservation);
        
        var parkingSpace = await _parkingSpaceRepositories.GetParkingSpaceByIdAsync(request.Id);
        if (parkingSpace != null)
        {
            parkingSpace.Status = "Available";
            await _parkingSpaceRepositories.UpdateParkingSpaceAsync(parkingSpace);
        }
        
    }
}