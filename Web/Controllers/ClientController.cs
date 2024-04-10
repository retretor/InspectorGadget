using System.Security.Claims;
using Application.Actions.Client;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
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
    private readonly IAppDbContextProvider _dbContextProvider;

    public ClientController(IMediator mediator, IAppDbContextProvider dbContextProvider)
    {
        _mediator = mediator;
        _dbContextProvider = dbContextProvider;
    }

    [HttpGet("{id}")]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.CLIENT, Role.ADMIN, Role.RECEPTIONIST })]
    public async Task<IActionResult> Get(int id)
    {
        if (User.Claims.First(c => c.Type == ClaimTypes.Role).Value == Role.CLIENT.ToString() &&
            User.Claims.Any(c => c.Type == ClaimTypes.NameIdentifier && c.Value != id.ToString()))
        {
            return BadRequest(Result.Failure(new AccessDeniedException()));
        }

        var dbContext = _dbContextProvider.GetDbContext(Utils.GetUserRole(User));
        if (dbContext == null)
        {
            return BadRequest(Result.Failure(new UnableToConnectToDatabaseException()));
        }

        var (result, entity) = await _mediator.Send(new GetClientQuery { Id = id, DbContext = dbContext });
        return result.Succeeded ? Ok(entity) : BadRequest(result);
    }

    [HttpGet]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.ADMIN, Role.RECEPTIONIST })]
    public async Task<IActionResult> GetAll([FromQuery] GetAllClientsQuery command)
    {
        var dbContext = _dbContextProvider.GetDbContext(Utils.GetUserRole(User));
        if (dbContext == null)
        {
            return BadRequest(Result.Failure(new UnableToConnectToDatabaseException()));
        }

        var (result, entities) = await _mediator.Send(command);
        return result.Succeeded ? Ok(entities) : BadRequest(result);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Create([FromQuery] CreateClientCommand command)
    {
        var dbContext = _dbContextProvider.GetDbContext(Utils.GetUserRole(User));
        if (dbContext == null)
        {
            return BadRequest(Result.Failure(new UnableToConnectToDatabaseException()));
        }

        command.DbContext = dbContext;
        var (result, id) = await _mediator.Send(command);
        if (result.Succeeded == false)
        {
            return BadRequest(result);
        }

        return CreatedAtAction(nameof(Get), new { id }, id);
    }

    [HttpPut("{id}")]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.CLIENT, Role.ADMIN, Role.RECEPTIONIST })]
    public async Task<IActionResult> Update([FromQuery] UpdateClientCommand command)
    {
        var dbContext = _dbContextProvider.GetDbContext(Utils.GetUserRole(User));
        if (dbContext == null)
        {
            return BadRequest(Result.Failure(new UnableToConnectToDatabaseException()));
        }

        command.DbContext = dbContext;
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

        var dbContext = _dbContextProvider.GetDbContext(Utils.GetUserRole(User));
        if (dbContext == null)
        {
            return BadRequest(Result.Failure(new UnableToConnectToDatabaseException()));
        }

        var result = await _mediator.Send(new DeleteClientCommand { Id = id, DbContext = dbContext});
        return result.Succeeded ? Ok() : BadRequest(result);  
    }
}