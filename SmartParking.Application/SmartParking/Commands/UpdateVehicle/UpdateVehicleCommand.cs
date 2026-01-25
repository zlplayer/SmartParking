using MediatR;
using SmartParking.Application.Dtos;

namespace SmartParking.Application.SmartParking.Commands.UpdateVehicle;

public class UpdateVehicleCommand: UpdateVehicleDto, IRequest
{
    public Guid Id { get; set; }
}