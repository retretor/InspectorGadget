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
public class RepairRequestController : ControllerBase
{
    private readonly RepairRequestService _service;

    public RepairRequestController(RepairRequestService service)
    {
        _service = service;
    }

    [RequiresClaim(ClaimTypes.Role, new[] {UserRole.ADMIN, UserRole.MASTER, UserRole.RECEPTIONIST})]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RepairRequest>>> GetRepairRequests()
    {
        var repairRequests = await _service.Get();
        if (repairRequests == null)
        {
            return NotFound();
        }

        return Ok(repairRequests);
    }

    [RequiresClaim(ClaimTypes.Role, new[] {UserRole.ADMIN, UserRole.MASTER, UserRole.RECEPTIONIST}, true)]
    [HttpGet("{id}")]
    public async Task<ActionResult<RepairRequest>> GetRepairRequest(int id)
    {
        var repairRequest = await _service.Get(id);

        if (repairRequest == null)
        {
            return NotFound();
        }

        return Ok(repairRequest);
    }

    [RequiresClaim(ClaimTypes.Role, new[] {UserRole.ADMIN, UserRole.MASTER, UserRole.RECEPTIONIST}, true)]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutRepairRequest(int id, RepairRequestDto repairRequestDto)
    {
        var updatedRepairRequest = await _service.Update(id, repairRequestDto);
        if (updatedRepairRequest == null)
        {
            return NotFound();
        }

        return NoContent();
    }

    [RequiresClaim(ClaimTypes.Role, new[] {UserRole.ADMIN, UserRole.CLIENT, UserRole.RECEPTIONIST})]
    [HttpPost]
    public async Task<ActionResult<RepairRequest>> PostRepairRequest(RepairRequestDto repairRequestDto)
    {
        var createdRepairRequest = await _service.Create(repairRequestDto);
        if (createdRepairRequest == null)
        {
            return BadRequest();
        }

        return CreatedAtAction("GetRepairRequest", new { id = createdRepairRequest.Id }, createdRepairRequest);
    }

    [RequiresClaim(ClaimTypes.Role, UserRole.ADMIN)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRepairRequest(int id)
    {
        var repairRequest = await _service.Get(id);
        if (repairRequest == null)
        {
            return NotFound();
        }

        await _service.Delete(id);

        return NoContent();
    }
}