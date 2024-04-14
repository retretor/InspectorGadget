﻿using System.Security.Claims;
using Application.Actions.RepairRequest;
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
public class RepairRequestController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IAppDbContextProvider _dbContextProvider;

    public RepairRequestController(IMediator mediator, IAppDbContextProvider dbContextProvider)
    {
        _mediator = mediator;
        _dbContextProvider = dbContextProvider;
    }

    [HttpGet("{id}")]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.CLIENT, Role.ADMIN, Role.RECEPTIONIST, Role.MASTER })]
    public async Task<IActionResult> Get(int id)
    {
        var dbContext = _dbContextProvider.GetDbContext(Utils.GetUserRole(User));
        if (dbContext == null)
        {
            return BadRequest(Result.Failure(new UnableToConnectToDatabaseException()));
        }

        var (result, entity) = await _mediator.Send(new GetRepairRequestQuery { Id = id, DbContext = dbContext });
        return result.Succeeded ? Ok(entity) : BadRequest(result);
    }

    [HttpGet]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.ADMIN, Role.RECEPTIONIST, Role.MASTER })]
    public async Task<IActionResult> GetAll([FromQuery] GetAllRepairRequestsQuery command)
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
    [RequiresClaim(ClaimTypes.Role, new[] { Role.CLIENT, Role.ADMIN, Role.RECEPTIONIST })]
    public async Task<IActionResult> Create([FromQuery] CreateRepairRequestCommand command)
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

    // TODO: maybe delete {id} from route
    [HttpPut("{id}")]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.CLIENT, Role.ADMIN, Role.RECEPTIONIST, Role.MASTER })]
    public async Task<IActionResult> Update([FromQuery] UpdateRepairRequestCommand command)
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
        var dbContext = _dbContextProvider.GetDbContext(Utils.GetUserRole(User));
        if (dbContext == null)
        {
            return BadRequest(Result.Failure(new UnableToConnectToDatabaseException()));
        }

        var result = await _mediator.Send(new DeleteRepairRequestCommand { Id = id, DbContext = dbContext });
        return result.Succeeded ? Ok() : BadRequest(result);
    }


    [HttpPut("ChangeStatus")]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.ADMIN, Role.RECEPTIONIST, Role.MASTER })]
    public async Task<IActionResult> ChangeStatus([FromQuery] ChangeRepairRequestStatusCommand command)
    {
        var dbContext = _dbContextProvider.GetDbContext(Utils.GetUserRole(User));
        if (dbContext == null)
        {
            return BadRequest(Result.Failure(new UnableToConnectToDatabaseException()));
        }

        command.DbContext = dbContext;
        var (result, succeed) = await _mediator.Send(command);
        return result.Succeeded ? Ok(succeed) : BadRequest(result);
    }

    [HttpGet("CalculateRepairCost")]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.CLIENT, Role.ADMIN, Role.RECEPTIONIST })]
    public async Task<IActionResult> CalculateRepairCost(int id)
    {
        var dbContext = _dbContextProvider.GetDbContext(Utils.GetUserRole(User));
        if (dbContext == null)
        {
            return BadRequest(Result.Failure(new UnableToConnectToDatabaseException()));
        }

        var (result, cost) = await _mediator.Send(new CalculateRepairRequestCostQuery
            { EntityId = id, DbContext = dbContext });
        return result.Succeeded ? Ok(cost) : BadRequest(result);
    }

    [HttpGet("CalculateRepairTime")]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.CLIENT, Role.ADMIN, Role.RECEPTIONIST })]
    public async Task<IActionResult> CalculateRepairTime(int id)
    {
        var dbContext = _dbContextProvider.GetDbContext(Utils.GetUserRole(User));
        if (dbContext == null)
        {
            return BadRequest(Result.Failure(new UnableToConnectToDatabaseException()));
        }

        var (result, cost) = await _mediator.Send(new CalculateRepairRequestTimeQuery
            { EntityId = id, DbContext = dbContext });
        return result.Succeeded ? Ok(cost) : BadRequest(result);
    }

    [HttpGet("IsAvailableAllParts")]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.ADMIN, Role.RECEPTIONIST, Role.MASTER })]
    public async Task<IActionResult> IsAvailableAllParts(int id)
    {
        var dbContext = _dbContextProvider.GetDbContext(Utils.GetUserRole(User));
        if (dbContext == null)
        {
            return BadRequest(Result.Failure(new UnableToConnectToDatabaseException()));
        }

        var (result, succeed) = await _mediator.Send(new IsAvailableAllPartsForRepairRequestQuery
            { EntityId = id, DbContext = dbContext });
        return result.Succeeded ? Ok(succeed) : BadRequest(result);
    }

    [HttpGet("GetRequestInfo")]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.CLIENT, Role.ADMIN, Role.RECEPTIONIST })]
    public async Task<IActionResult> GetRequestInfo(int id)
    {
        var dbContext = _dbContextProvider.GetDbContext(Utils.GetUserRole(User));
        if (dbContext == null)
        {
            return BadRequest(Result.Failure(new UnableToConnectToDatabaseException()));
        }

        var (result, info) = await _mediator.Send(new GetRepairRequestInfoQuery
            { EntityId = id, DbContext = dbContext });
        return result.Succeeded ? Ok(info) : BadRequest(result);
    }
}