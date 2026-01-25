using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartParking.Application.Dtos;
using SmartParking.Application.SmartParking.Commands.CreateParkingSpace;
using SmartParking.Application.SmartParking.Commands.DeleteParkingZone;
using SmartParking.Application.SmartParking.Commands.UpdateParkingSpace;
using SmartParking.Application.SmartParking.Queries.GetAllParkingSpace;
using SmartParking.Application.SmartParking.Queries.GetParkingSpace;

namespace SmartParking.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParkingSpaceController : ControllerBase
{
    private readonly IMediator _mediator;

    public ParkingSpaceController(IMediator mediator)
    {
        _mediator=mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllParkingSpace()
    {
        var parkingSpace = await _mediator.Send(new GetAllParkingSpaceQuery());
        return Ok(parkingSpace);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetParkingSpace(Guid id)
    {
        var parkingSpace = await _mediator.Send(new GetParkingSpaceQuery{Id = id});
        return Ok(parkingSpace);
    }

    [HttpPost]
    public async Task<IActionResult> CreateParkingSpace(CreateParkingSpaceCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateParkingSpace(Guid id, UpdateParkingSpaceCommand command)
    {
        command.Id = id;
        await _mediator.Send(command);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteParkingSpace(Guid id)
    {
        await _mediator.Send(new DeleteParkingZoneCommand{Id = id});
        return Ok();
    }
}