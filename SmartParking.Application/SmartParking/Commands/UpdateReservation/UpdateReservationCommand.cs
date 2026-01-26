using MediatR;
using SmartParking.Application.Dtos;

namespace SmartParking.Application.SmartParking.Commands.UpdateReservation;

public class UpdateReservationCommand: UpdateReservationDto, IRequest
{
    public Guid Id { get; set; }
}