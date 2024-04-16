using System.Security.Claims;
using Application.Actions.RepairPart;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Identity;

namespace Web.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class RepairPartController : ControllerBase
{
    private readonly IMediator _mediator;

    public RepairPartController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.MASTER, Role.ADMIN })]
    public async Task<IActionResult> Get(int id)
    {
        var (result, entity) = await _mediator.Send(new GetRepairPartQuery { Id = id });
        return result.Succeeded ? Ok(entity) : BadRequest(result);
    }

    [HttpGet]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.MASTER, Role.ADMIN })]
    public async Task<IActionResult> GetAll([FromQuery] GetAllRepairPartsQuery command)
    {
        var (result, entities) = await _mediator.Send(command);
        return result.Succeeded ? Ok(entities) : BadRequest(result);
    }

    [HttpPost]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.ADMIN })]
    public async Task<IActionResult> Create([FromQuery] CreateRepairPartCommand command)
    {
        var (result, id) = await _mediator.Send(command);
        return result.Succeeded == false ? BadRequest(result) : CreatedAtAction(nameof(Get), new { id }, id);
    }

    [HttpPut]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.ADMIN, Role.MASTER })]
    public async Task<IActionResult> Update([FromQuery] UpdateRepairPartCommand command)
    {
        var result = await _mediator.Send(command);
        return result.Succeeded ? Ok() : BadRequest(result);
    }

    [HttpDelete("{id}")]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.ADMIN })]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteRepairPartCommand { Id = id });
        return result.Succeeded ? Ok() : BadRequest(result);
    }
}