using System.Security.Claims;
using Application.Actions.AllowedRepairTypesForEmployee;
using Domain.Entities.Basic;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Identity;

namespace Web.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]

// TODO: Add roles to the endpoints
public class AllowedRepairTypesForEmployeeController : ControllerBase
{
    private readonly IMediator _mediator;

    public AllowedRepairTypesForEmployeeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.CLIENT, Role.ADMIN, Role.RECEPTIONIST })]
    public async Task<AllowedRepairTypesForEmployee> Get(int id)
    {
        return await _mediator.Send(new GetAllowedRepairTypesForEmployeeQuery(id));
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IEnumerable<AllowedRepairTypesForEmployee>> GetAll([FromQuery] GetAllAllowedRepairTypesForEmployeeQuery allowedRepairTypesForEmployeeQuery)
    {
        return await _mediator.Send(allowedRepairTypesForEmployeeQuery);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<int> Create([FromQuery] CreateAllowedRepairTypesForEmployeeQuery command)
    {
        return await _mediator.Send(command);
    }

    [HttpPut("{id}")]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.ADMIN })]
    public async Task<IResult> Update(int id, [FromQuery] UpdateAllowedRepairTypesForEmployeeQuery command)
    {
        if (id != command.Id) return Results.BadRequest();
        await _mediator.Send(command);
        return Results.NoContent();
    }

    [HttpDelete("{id}")]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.ADMIN })]
    public async Task<IResult> Delete(int id)
    {
        await _mediator.Send(new DeleteAllowedRepairTypesForEmployeeQuery(id));
        return Results.NoContent();
    }
}