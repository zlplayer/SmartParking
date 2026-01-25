using MediatR;
using SmartParking.Application.Dtos;

namespace SmartParking.Application.SmartParking.Commands.CreateParkingSpace;

public class CreateParkingSpaceCommand: CreateParkingSpaceDto, IRequest
{
    
}