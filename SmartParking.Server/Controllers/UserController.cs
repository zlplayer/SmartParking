using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartParking.Application.SmartParking.Queries.GetUser;

namespace SmartParking.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController: ControllerBase
{
    private readonly IMediator _mediator;
    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        var userDto = await _mediator.Send(new GetUserQuery{UserId = id});
        return Ok(userDto);
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        var userId = Guid.Parse(userIdClaim.Value);

        var userDto = await _mediator.Send(new GetUserQuery{ UserId = userId });
        return Ok(userDto);
    }
}