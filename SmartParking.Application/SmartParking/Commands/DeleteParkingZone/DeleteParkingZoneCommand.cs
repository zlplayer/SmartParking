using MediatR;

namespace SmartParking.Application.SmartParking.Commands.DeleteParkingZone;

public class DeleteParkingZoneCommand: IRequest
{
    public Guid Id { get; set; }
}