using MediatR;

namespace SmartParking.Application.SmartParking.Commands.DeleteReservation;

public class DeleteReservationCommand:IRequest
{
    public Guid Id { get; set; }
}