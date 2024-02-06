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
public class RepairPartController : ControllerBase
{
    private readonly RepairPartService _service;

    public RepairPartController(RepairPartService service)
    {
        _service = service;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RepairPart>>> GetRepairParts()
    {
        var repairParts = await _service.Get();
        if (repairParts == null)
        {
            return NotFound();
        }

        return Ok(repairParts);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<RepairPart>> GetRepairPart(int id)
    {
        var repairPart = await _service.Get(id);

        if (repairPart == null)
        {
            return NotFound();
        }

        return Ok(repairPart);
    }

    [RequiresClaim(ClaimTypes.Role, UserRole.ADMIN)]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutRepairPart(int id, RepairPartDto repairPartDto)
    {
        var updatedRepairPart = await _service.Update(id, repairPartDto);
        if (updatedRepairPart == null)
        {
            return NotFound();
        }

        return NoContent();
    }

    [RequiresClaim(ClaimTypes.Role, UserRole.ADMIN)]
    [HttpPost]
    public async Task<ActionResult<RepairPart>> PostRepairPart(RepairPartDto repairPartDto)
    {
        var createdRepairPart = await _service.Create(repairPartDto);
        if (createdRepairPart == null)
        {
            return BadRequest();
        }

        return CreatedAtAction("GetRepairPart", new { id = createdRepairPart.Id }, createdRepairPart);
    }

    [RequiresClaim(ClaimTypes.Role, UserRole.ADMIN)]
    [HttpDelete("{id}")]
    public async Task<ActionResult<RepairPart>> DeleteRepairPart(int id)
    {
        var repairPart = await _service.Get(id);
        if (repairPart == null)
        {
            return NotFound();
        }
        
        await _service.Delete(id);
        
        return Ok(repairPart);
    }
}