using AutoMapper;
using MediatR;
using SmartParking.Domain.Entities;
using SmartParking.Domain.Interfaces;

namespace SmartParking.Application.SmartParking.Commands.CreateReservation;

public class CreateReservationCommandHandler: IRequestHandler<CreateReservationCommand>
{
    private readonly IMapper _mapper;
    private readonly IReservationRepositories _reservationRepositories;
    private readonly IParkingSpaceRepositories _parkingSpaceRepositories;

    public CreateReservationCommandHandler(IMapper mapper, IReservationRepositories reservationRepositories, IParkingSpaceRepositories parkingSpaceRepositories)
    {
        _mapper = mapper;
        _reservationRepositories = reservationRepositories;
        _parkingSpaceRepositories = parkingSpaceRepositories;
    }
    public async Task Handle(CreateReservationCommand request, CancellationToken cancellationToken)
    {
        var reservation = _mapper.Map<CreateReservationCommand, Reservation>(request);
        await _reservationRepositories.CreateReservationAsync(reservation);

        var parkingSpace = await _parkingSpaceRepositories.GetParkingSpaceByIdAsync(request.ParkingSpaceId);
        if (parkingSpace != null)
        {
            parkingSpace.Status = "Reserved";
            await _parkingSpaceRepositories.UpdateParkingSpaceAsync(parkingSpace);
        }
    }
}