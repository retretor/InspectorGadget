using System.Security.Claims;
using Application.Actions.RepairRequest;
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

    public RepairRequestController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.CLIENT, Role.ADMIN, Role.RECEPTIONIST, Role.MASTER })]
    public async Task<IActionResult> Get(int id)
    {
        var (result, entity) = await _mediator.Send(new GetRepairRequestQuery { Id = id });
        return result.Succeeded ? Ok(entity) : BadRequest(result);
    }

    [HttpGet]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.ADMIN, Role.RECEPTIONIST, Role.MASTER, Role.CLIENT })]
    public async Task<IActionResult> GetAll([FromQuery] GetAllRepairRequestsQuery command)
    {
        var (result, entities) = await _mediator.Send(command);
        return result.Succeeded ? Ok(entities) : BadRequest(result);
    }

    [HttpPost]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.CLIENT, Role.ADMIN, Role.RECEPTIONIST })]
    public async Task<IActionResult> Create([FromQuery] CreateRepairRequestCommand command)
    {
        var (result, id) = await _mediator.Send(command);
        return result.Succeeded == false ? BadRequest(result) : CreatedAtAction(nameof(Get), new { id }, id);
    }

    [HttpPut]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.CLIENT, Role.ADMIN, Role.RECEPTIONIST, Role.MASTER })]
    public async Task<IActionResult> Update([FromQuery] UpdateRepairRequestCommand command)
    {
        var result = await _mediator.Send(command);
        return result.Succeeded ? Ok() : BadRequest(result);
    }

    [HttpDelete("{id}")]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.CLIENT, Role.ADMIN, Role.RECEPTIONIST })]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteRepairRequestCommand { Id = id });
        return result.Succeeded ? Ok() : BadRequest(result);
    }


    [HttpPut("ChangeStatus")]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.ADMIN, Role.RECEPTIONIST, Role.MASTER })]
    public async Task<IActionResult> ChangeStatus([FromQuery] ChangeRepairRequestStatusCommand command)
    {
        var (result, succeed) = await _mediator.Send(command);
        return result.Succeeded ? Ok(succeed) : BadRequest(result);
    }

    [HttpGet("CalculateRepairCost")]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.CLIENT, Role.ADMIN, Role.RECEPTIONIST })]
    public async Task<IActionResult> CalculateRepairCost(int id)
    {
        var (result, cost) = await _mediator.Send(new CalculateRepairRequestCostQuery
            { EntityId = id });
        return result.Succeeded ? Ok(cost) : BadRequest(result);
    }

    [HttpGet("CalculateRepairTime")]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.CLIENT, Role.ADMIN, Role.RECEPTIONIST })]
    public async Task<IActionResult> CalculateRepairTime(int id)
    {
        var (result, cost) = await _mediator.Send(new CalculateRepairRequestTimeQuery
            { EntityId = id });
        return result.Succeeded ? Ok(cost) : BadRequest(result);
    }

    [HttpGet("IsAvailableAllParts")]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.ADMIN, Role.RECEPTIONIST, Role.MASTER })]
    public async Task<IActionResult> IsAvailableAllParts(int id)
    {
        var (result, succeed) = await _mediator.Send(new IsAvailableAllPartsForRepairRequestQuery
            { EntityId = id });
        return result.Succeeded ? Ok(succeed) : BadRequest(result);
    }

    [HttpGet("GetRequestInfo")]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.CLIENT, Role.ADMIN, Role.RECEPTIONIST })]
    public async Task<IActionResult> GetRequestInfo(int id)
    {
        var (result, info) = await _mediator.Send(new GetRepairRequestInfoQuery
            { EntityId = id });
        return result.Succeeded ? Ok(info) : BadRequest(result);
    }
}