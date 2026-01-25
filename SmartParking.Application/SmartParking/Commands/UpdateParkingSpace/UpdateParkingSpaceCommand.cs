using MediatR;
using SmartParking.Application.Dtos;

namespace SmartParking.Application.SmartParking.Commands.UpdateParkingSpace;

public class UpdateParkingSpaceCommand: UpdateParkingSpaceDto, IRequest
{
    public Guid Id { get; set; }
}