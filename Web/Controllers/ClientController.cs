using System.Security.Claims;
using Application.Actions.Authorization.AuthorizeByLogin;
using Application.Actions.Client;
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
public class ClientController : ControllerBase
{
    private readonly IMediator _mediator;

    public ClientController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.CLIENT, Role.ADMIN, Role.RECEPTIONIST })]
    public async Task<IActionResult> Get(int id)
    {
        var userId = User.Claims.First(c => c.Type == ClaimTypes.Sid).Value;
        Console.WriteLine($"User with id: {userId} tried to access to client with id: {id}");
        if (User.Claims.First(c => c.Type == ClaimTypes.Role).Value == Role.CLIENT.ToString() &&
            User.Claims.Any(c => c.Type == ClaimTypes.Sid && c.Value != id.ToString()))
        {
            Console.WriteLine("Access denied");
            return BadRequest(Result.Failure(new AccessDeniedException()));
        }

        var (result, entity) = await _mediator.Send(new GetClientQuery { DbUserId = id });
        return result.Succeeded ? Ok(entity) : BadRequest(result);
    }

    [HttpGet]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.ADMIN, Role.RECEPTIONIST })]
    public async Task<IActionResult> GetAll([FromQuery] GetAllClientsQuery command)
    {
        var (result, entities) = await _mediator.Send(command);
        return result.Succeeded ? Ok(entities) : BadRequest(result);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Create([FromQuery] CreateClientCommand command)
    {
        var (result, id) = await _mediator.Send(command);
        if (result.Succeeded == false)
        {
            return BadRequest(result);
        }

        var authorizeByLoginQuery = new AuthorizeByLoginQuery
        {
            Login = command.Login,
            Password = command.PasswordHash
        };
        (result, var response) = await _mediator.Send(authorizeByLoginQuery);

        return result.Succeeded ? Ok(response) : BadRequest(result);
    }

    [HttpPut]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.CLIENT, Role.ADMIN, Role.RECEPTIONIST })]
    public async Task<IActionResult> Update([FromQuery] UpdateClientCommand command)
    {
        var result = await _mediator.Send(command);
        return result.Succeeded ? Ok() : BadRequest(result);
    }

    [HttpDelete("{id}")]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.CLIENT, Role.ADMIN, Role.RECEPTIONIST })]
    public async Task<IActionResult> Delete(int id)
    {
        if (User.Claims.First(c => c.Type == ClaimTypes.Role).Value == Role.CLIENT.ToString() &&
            User.Claims.Any(c => c.Type == ClaimTypes.NameIdentifier && c.Value != id.ToString()))
        {
            return BadRequest(Result.Failure(new AccessDeniedException()));
        }

        var result = await _mediator.Send(new DeleteClientCommand { Id = id });
        return result.Succeeded ? Ok() : BadRequest(result);
    }
}