using MediatR;
using SmartParking.Application.Dtos;

namespace SmartParking.Application.SmartParking.Commands.UpdateParkingZone;

public class UpdateParkingZoneCommand: UpdateParkingZoneDto, IRequest
{
    public Guid Id { get; set; }
}