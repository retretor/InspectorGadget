using System.Security.Claims;
using Application.Actions.Employee;
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
public class EmployeeController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IAppDbContextProvider _dbContextProvider;

    public EmployeeController(IMediator mediator, IAppDbContextProvider dbContextProvider)
    {
        _mediator = mediator;
        _dbContextProvider = dbContextProvider;
    }

    [HttpGet("{id}")]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.MASTER, Role.ADMIN, Role.RECEPTIONIST })]
    public async Task<IActionResult> Get(int id)
    {
        var dbContext = _dbContextProvider.GetDbContext(Utils.GetUserRole(User));
        if (dbContext == null)
        {
            return BadRequest(Result.Failure(new UnableToConnectToDatabaseException()));
        }

        var (result, entity) = await _mediator.Send(new GetEmployeeQuery { Id = id, DbContext = dbContext });
        return result.Succeeded ? Ok(entity) : BadRequest(result);
    }

    [HttpGet]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.MASTER, Role.ADMIN, Role.RECEPTIONIST })]
    public async Task<IActionResult> GetAll([FromQuery] GetAllEmployeesQuery command)
    {
        var dbContext = _dbContextProvider.GetDbContext(Utils.GetUserRole(User));
        if (dbContext == null)
        {
            return BadRequest(Result.Failure(new UnableToConnectToDatabaseException()));
        }

        command.DbContext = dbContext;
        var (result, entities) = await _mediator.Send(command);
        return result.Succeeded ? Ok(entities) : BadRequest(result);
    }

    [HttpPost]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.ADMIN })]
    public async Task<IActionResult> Create([FromQuery] CreateEmployeeCommand command)
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

    [HttpPut]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.ADMIN })]
    public async Task<IActionResult> Update([FromQuery] UpdateEmployeeCommand command)
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

    [HttpPut("UpdateRole")]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.ADMIN })]
    public async Task<IActionResult> UpdateRole([FromQuery] UpdateEmployeeRoleCommand command)
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
    [RequiresClaim(ClaimTypes.Role, new[] { Role.ADMIN })]
    public async Task<IActionResult> Delete(int id)
    {
        var dbContext = _dbContextProvider.GetDbContext(Utils.GetUserRole(User));
        if (dbContext == null)
        {
            return BadRequest(Result.Failure(new UnableToConnectToDatabaseException()));
        }

        var result = await _mediator.Send(new DeleteEmployeeCommand { Id = id, DbContext = dbContext });
        return result.Succeeded ? Ok() : BadRequest(result);
    }

    [HttpGet("GetMasterRanking")]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.ADMIN })]
    public async Task<IActionResult> GetMasterRanking([FromQuery] GetMasterRankingQuery command)
    {
        var dbContext = _dbContextProvider.GetDbContext(Utils.GetUserRole(User));
        if (dbContext == null)
        {
            return BadRequest(Result.Failure(new UnableToConnectToDatabaseException()));
        }

        command.DbContext = dbContext;
        var (result, entities) = await _mediator.Send(command);
        return result.Succeeded ? Ok(entities) : BadRequest(result);
    }
}