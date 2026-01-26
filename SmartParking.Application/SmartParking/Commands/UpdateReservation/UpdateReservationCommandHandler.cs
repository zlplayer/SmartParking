using AutoMapper;
using MediatR;
using SmartParking.Domain.Interfaces;

namespace SmartParking.Application.SmartParking.Commands.UpdateReservation;

public class UpdateReservationCommandHandler: IRequestHandler<UpdateReservationCommand>
{
    private readonly IMapper _mapper;
    private readonly IReservationRepositories _reservationRepositories;

    public UpdateReservationCommandHandler(IMapper mapper, IReservationRepositories reservationRepositories)
    {
        _mapper = mapper;
        _reservationRepositories = reservationRepositories;
    }
    public async Task Handle(UpdateReservationCommand request, CancellationToken cancellationToken)
    {
        var reservation = await _reservationRepositories.GetReservationByIdAsync(request.Id);
        if (reservation == null) throw new KeyNotFoundException();
        _mapper.Map(request, reservation);
        await _reservationRepositories.UpdateReservationAsync(reservation);
    }
}