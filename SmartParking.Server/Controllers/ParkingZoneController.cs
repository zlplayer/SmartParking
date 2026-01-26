using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartParking.Application.SmartParking.Commands.CreateParkingZone;
using SmartParking.Application.SmartParking.Commands.DeleteParkingZone;
using SmartParking.Application.SmartParking.Commands.UpdateParkingZone;
using SmartParking.Application.SmartParking.Queries.GetAllParkingZones;
using SmartParking.Application.SmartParking.Queries.GetParkingZone;

namespace SmartParking.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParkingZoneController : ControllerBase
{
    private readonly IMediator _mediator;

    public ParkingZoneController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllParkingZone()
    {
        var parkingZone = await _mediator.Send(new GetAllParkingZonesQuery());
        return Ok(parkingZone);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetParkingZone(Guid id)
    {
        var parkingZone = await _mediator.Send(new GetParkingZoneQuery{Id = id});
        return Ok(parkingZone);
    }

    [HttpPost]
    public async Task<IActionResult> CreateParkingZone(CreateParkingZoneCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateParkingZone(Guid id, UpdateParkingZoneCommand command)
    {
        command.Id = id;
        await _mediator.Send(command);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteParkingZone(Guid id)
    {
        await _mediator.Send(new DeleteParkingZoneCommand { Id = id });
        return Ok();
    }
}