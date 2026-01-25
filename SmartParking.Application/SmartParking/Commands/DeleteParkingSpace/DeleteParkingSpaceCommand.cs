using MediatR;

namespace SmartParking.Application.SmartParking.Commands.DeleteParkingSpace;

public class DeleteParkingSpaceCommand: IRequest
{
    public Guid Id { get; set; }
}