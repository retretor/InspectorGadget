﻿using System.Security.Claims;
using Application.Actions.Device;
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

// TODO: maybe move dbContext logic to a DbContextMiddleware or AuthenticationBehavior
public class DeviceController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IAppDbContextProvider _dbContextProvider;

    public DeviceController(IMediator mediator, IAppDbContextProvider dbContextProvider)
    {
        _mediator = mediator;
        _dbContextProvider = dbContextProvider;
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(int id)
    {
        var dbContext = _dbContextProvider.GetDbContext(Utils.GetUserRole(User));
        if (dbContext == null)
        {
            return BadRequest(Result.Failure(new UnableToConnectToDatabaseException()));
        }

        var (result, entity) = await _mediator.Send(new GetDeviceQuery { Id = id, DbContext = dbContext });
        return result.Succeeded ? Ok(entity) : BadRequest(result);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll([FromQuery] GetAllDevicesQuery command)
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
    public async Task<IActionResult> Create([FromQuery] CreateDeviceCommand command)
    {
        var dbContext = _dbContextProvider.GetDbContext(Utils.GetUserRole(User));
        if (dbContext == null)
        {
            return BadRequest(Result.Failure(new UnableToConnectToDatabaseException()));
        }

        command.DbContext = dbContext;
        var (result, id) = await _mediator.Send(command);
        if (!result.Succeeded)
        {
            return BadRequest(result);
        }

        return CreatedAtAction(nameof(Get), new { id }, id);
    }

    [HttpPut("{id}")]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.ADMIN })]
    public async Task<IActionResult> Update([FromQuery] UpdateDeviceCommand command)
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

        var result = await _mediator.Send(new DeleteDeviceCommand { Id = id, DbContext = dbContext });
        return result.Succeeded ? Ok() : BadRequest(result);
    }

    // GetRepairTypesInfo
    [HttpGet("RepairTypesInfo")]
    [AllowAnonymous]
    public async Task<IActionResult> GetRepairTypesInfo(int id)
    {
        var dbContext = _dbContextProvider.GetDbContext(Utils.GetUserRole(User));
        if (dbContext == null)
        {
            return BadRequest(Result.Failure(new UnableToConnectToDatabaseException()));
        }

        var (result, entity) = await _mediator.Send(new GetRepairTypesInfoForDeviceQuery
            { EntityId = id, DbContext = dbContext });
        return result.Succeeded ? Ok(entity) : BadRequest(result);
    }
}