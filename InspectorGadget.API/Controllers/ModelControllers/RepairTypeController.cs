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
public class RepairTypeController : ControllerBase
{
    private readonly RepairTypeService _service;

    public RepairTypeController(RepairTypeService service)
    {
        _service = service;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RepairType>>> GetRepairTypes()
    {
        var repairTypes = await _service.Get();
        if (repairTypes == null)
        {
            return NotFound();
        }

        return Ok(repairTypes);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<RepairType>> GetRepairType(int id)
    {
        var repairType = await _service.Get(id);

        if (repairType == null)
        {
            return NotFound();
        }

        return Ok(repairType);
    }

    [RequiresClaim(ClaimTypes.Role, UserRole.ADMIN)]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutRepairType(int id, RepairTypeDto repairTypeDto)
    {
        var updatedRepairType = await _service.Update(id, repairTypeDto);
        if (updatedRepairType == null)
        {
            return NotFound();
        }

        return NoContent();
    }

    [RequiresClaim(ClaimTypes.Role, UserRole.ADMIN)]
    [HttpPost]
    public async Task<ActionResult<RepairType>> PostRepairType(RepairTypeDto repairType)
    {
        var createdRepairType = await _service.Create(repairType);
        if (createdRepairType == null)
        {
            return BadRequest();
        }

        return CreatedAtAction("GetRepairType", new { id = createdRepairType.Id }, createdRepairType);
    }

    [RequiresClaim(ClaimTypes.Role, UserRole.ADMIN)]
    [HttpDelete("{id}")]
    public async Task<ActionResult<RepairType>> DeleteRepairType(int id)
    {
        var repairType = await _service.Get(id);
        if (repairType == null)
        {
            return NotFound();
        }

        await _service.Delete(id);

        return Ok(repairType);
    }
}