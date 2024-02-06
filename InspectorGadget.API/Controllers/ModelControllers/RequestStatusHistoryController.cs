using System.Security.Claims;
using InspectorGadget.DTOs;
using InspectorGadget.Identity;
using InspectorGadget.Models;
using InspectorGadget.Services.ModelServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InspectorGadget.Controllers.ModelControllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class RequestStatusHistoryController : ControllerBase
{
    private readonly RequestStatusHistoryService _service;

    public RequestStatusHistoryController(RequestStatusHistoryService service)
    {
        _service = service;
    }

    [RequiresClaim(ClaimTypes.Role, new[] {UserRole.ADMIN, UserRole.MASTER, UserRole.RECEPTIONIST})]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RequestStatusHistory>>> GetRequestStatusHistories()
    {
        var requestStatusHistories = await _service.Get();
        if (requestStatusHistories == null)
        {
            return NotFound();
        }

        return Ok(requestStatusHistories);
    }

    [RequiresClaim(ClaimTypes.Role, new[] {UserRole.ADMIN, UserRole.MASTER, UserRole.RECEPTIONIST}, true)]
    [HttpGet("{id}")]
    public async Task<ActionResult<RequestStatusHistory>> GetRequestStatusHistory(int id)
    {
        var requestStatusHistory = await _service.Get(id);

        if (requestStatusHistory == null)
        {
            return NotFound();
        }

        return Ok(requestStatusHistory);
    }

    [RequiresClaim(ClaimTypes.Role, new[] {UserRole.ADMIN, UserRole.MASTER, UserRole.RECEPTIONIST})]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutRequestStatusHistory(int id, RequestStatusHistoryDto requestStatusHistoryDto)
    {
        var updatedRequestStatusHistory = await _service.Update(id, requestStatusHistoryDto);
        if (updatedRequestStatusHistory == null)
        {
            return NotFound();
        }

        return NoContent();
    }

    [RequiresClaim(ClaimTypes.Role, new[] {UserRole.ADMIN, UserRole.MASTER, UserRole.RECEPTIONIST, UserRole.CLIENT})]
    [HttpPost]
    public async Task<ActionResult<RequestStatusHistory>> PostRequestStatusHistory(
        RequestStatusHistoryDto requestStatusHistoryDto)
    {
        var createdRequestStatusHistory = await _service.Create(requestStatusHistoryDto);
        if (createdRequestStatusHistory == null)
        {
            return BadRequest();
        }

        return CreatedAtAction("GetRequestStatusHistory", new { id = createdRequestStatusHistory.Id },
            createdRequestStatusHistory);
    }

    [RequiresClaim(ClaimTypes.Role, UserRole.ADMIN)]
    [HttpDelete("{id}")]
    public async Task<ActionResult<RequestStatusHistory>> DeleteRequestStatusHistory(int id)
    {
        var requestStatusHistory = await _service.Get(id);
        if (requestStatusHistory == null)
        {
            return NotFound();
        }

        await _service.Delete(id);

        return Ok(requestStatusHistory);
    }
}