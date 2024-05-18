using System.Security.Claims;
using Application.Actions.DbUser;
using Application.Common.Exceptions;
using Application.Common.Models;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Identity;

namespace Web.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class DbUserController : ControllerBase
{
    private readonly IMediator _mediator;

    public DbUserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // TODO: Change this method
    [HttpGet("{id}")]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.ADMIN, Role.CLIENT })]
    public async Task<IActionResult> Get(int id)
    {
        var userId = User.Claims.First(c => c.Type == ClaimTypes.Sid).Value;
        Console.WriteLine($"User with id: {userId} tried to access to db user with id: {id}");
        if (User.Claims.First(c => c.Type == ClaimTypes.Role).Value == Role.CLIENT.ToString() &&
            User.Claims.Any(c => c.Type == ClaimTypes.Sid && c.Value != id.ToString()))
        {
            Console.WriteLine("Access denied");
            return BadRequest(Result.Failure(new AccessDeniedException()));
        }

        var (result, entity) = await _mediator.Send(new GetDbUserQuery { Id = id });
        return result.Succeeded ? Ok(entity) : BadRequest(result);
    }

    [HttpGet]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.ADMIN })]
    public async Task<IActionResult> GetAll([FromQuery] GetAllDbUsersQuery command)
    {
        var (result, entities) = await _mediator.Send(command);
        return result.Succeeded ? Ok(entities) : BadRequest(result);
    }

    [HttpPut]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.ADMIN })]
    public async Task<IActionResult> Update([FromQuery] UpdateDbUserCommand command)
    {
        var result = await _mediator.Send(command);
        return result.Succeeded ? Ok() : BadRequest(result);
    }

    [HttpDelete("{id}")]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.ADMIN })]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteDbUserCommand { Id = id });
        return result.Succeeded ? Ok() : BadRequest(result);
    }
}