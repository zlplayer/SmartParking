using AutoMapper;
using MediatR;
using SmartParking.Domain.Entities;
using SmartParking.Domain.Interfaces;

namespace SmartParking.Application.SmartParking.Commands.CreateReservation;

public class CreateReservationCommandHandler: IRequestHandler<CreateReservationCommand>
{
    private readonly IMapper _mapper;
    private readonly IReservationRepositories _reservationRepositories;

    public CreateReservationCommandHandler(IMapper mapper, IReservationRepositories reservationRepositories)
    {
        _mapper = mapper;
        _reservationRepositories = reservationRepositories;
    }
    public async Task Handle(CreateReservationCommand request, CancellationToken cancellationToken)
    {
        var reservation = _mapper.Map<CreateReservationCommand, Reservation>(request);
        await _reservationRepositories.CreateReservationAsync(reservation);
    }
}