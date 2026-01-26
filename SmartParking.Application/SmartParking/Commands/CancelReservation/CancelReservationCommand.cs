using MediatR;
using SmartParking.Application.Dtos;

namespace SmartParking.Application.SmartParking.Commands.CancelReservation;

public class CancelReservationCommand: CancelReservationDto, IRequest
{
    public Guid Id { get; set; }
}