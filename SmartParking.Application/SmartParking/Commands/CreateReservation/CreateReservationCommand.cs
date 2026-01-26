using MediatR;
using SmartParking.Application.Dtos;

namespace SmartParking.Application.SmartParking.Commands.CreateReservation;

public class CreateReservationCommand: CreateReservationDto, IRequest
{
    
}