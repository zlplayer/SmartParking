using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartParking.Application.SmartParking.Commands.CreateVehicle;
using SmartParking.Application.SmartParking.Commands.DeleteVehicle;
using SmartParking.Application.SmartParking.Commands.UpdateVehicle;
using SmartParking.Application.SmartParking.Queries.GetAllVehicleByUserId;

namespace SmartParking.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehicleController : ControllerBase
{
    private readonly IMediator _mediator;

    public VehicleController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetAllVehiclesByUserId(Guid userId)
    {
        var vehicles = await _mediator.Send(new GetAllVehicleByUserIdQuery{UserId= userId});
        return Ok(vehicles);
    }

    [HttpPost]
    public async Task<IActionResult> CreateVehicle([FromBody] CreateVehicleCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }

    [HttpPut("{vehicleId}")]
    public async Task<IActionResult> UpdateVehicle(Guid vehicleId, [FromBody] UpdateVehicleCommand command)
    {
        command.Id = vehicleId;
        await _mediator.Send(command);
        return Ok();
    }

    [HttpDelete("{vehicleId}")]
    public async Task<IActionResult> DeleteVehicle(Guid vehicleId)
    {
        await _mediator.Send(new DeleteVehicleCommand{Id=vehicleId});
        return Ok();
    }
}