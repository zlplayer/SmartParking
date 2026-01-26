using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartParking.Application.SmartParking.Commands.CancelReservation;
using SmartParking.Application.SmartParking.Commands.CreateReservation;
using SmartParking.Application.SmartParking.Commands.DeleteReservation;
using SmartParking.Application.SmartParking.Commands.UpdateReservation;
using SmartParking.Application.SmartParking.Queries.GetAllReservations;
using SmartParking.Application.SmartParking.Queries.GetReservationById;
using SmartParking.Application.SmartParking.Queries.GetReservationByUserId;

namespace SmartParking.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReservationController(IMediator  mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllReservation()
    {
        var reservation = await _mediator.Send(new GetAllReservationQuery());
        return Ok(reservation);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetReservationById(Guid id)
    {
        var reservation = await _mediator.Send(new GetReservationByIdQuery{Id = id});
        return Ok(reservation);
    }
    
    [HttpGet("User/{id}")]
    public async Task<IActionResult> GetReservationByUserId(Guid id)
    {
        var reservation = await _mediator.Send(new GetReservationByUserIdQuery{UserId = id});
        return Ok(reservation);
    }

    [HttpPost]
    public async Task<IActionResult> CreateReservation([FromBody] CreateReservationCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> CreateReservation(Guid id,[FromBody] UpdateReservationCommand command)
    {
        command.Id = id;
        await _mediator.Send(command);
        return Ok();
    }

    [HttpPut("CancelReservation/{id}")]
    public async Task<IActionResult> CancelReservation(Guid id, [FromBody] CancelReservationCommand command)
    {
        command.Id = id;
        await _mediator.Send(command);
        return Ok();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReservation(Guid id)
    {
        await _mediator.Send(new DeleteReservationCommand { Id = id });
        return Ok();
    }
}