using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartParking.Application.SmartParking.Commands.LoginCommand;
using SmartParking.Application.SmartParking.Commands.RegisterCommand;

namespace SmartParking.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController:ControllerBase
{
    private readonly IMediator _mediator;
    
    public AuthController(IMediator mediator)
    {
        _mediator = mediator;

    }
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }
}