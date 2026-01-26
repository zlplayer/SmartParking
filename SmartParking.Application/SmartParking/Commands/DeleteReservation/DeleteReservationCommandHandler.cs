using MediatR;
using SmartParking.Domain.Interfaces;

namespace SmartParking.Application.SmartParking.Commands.DeleteReservation;

public class DeleteReservationCommandHandler: IRequestHandler<DeleteReservationCommand>
{
    private readonly IReservationRepositories _reservationRepositories;

    public DeleteReservationCommandHandler(IReservationRepositories reservationRepositories)
    {
        _reservationRepositories = reservationRepositories;
    }
    public async Task Handle(DeleteReservationCommand request, CancellationToken cancellationToken)
    {
        var reservation = await _reservationRepositories.GetReservationByIdAsync(request.Id);
        if (reservation == null) throw new KeyNotFoundException();
        await _reservationRepositories.DeleteReservationAsync(reservation);
    }
}