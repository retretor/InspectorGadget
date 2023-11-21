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
public class PartForRepairPartController : ControllerBase
{
    private readonly IEntityService<PartForRepairPart, PartForRepairPartDto> _service;

    public PartForRepairPartController(PartForRepairPartService service)
    {
        _service = service;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PartForRepairPart>>> GetPartForRepairParts()
    {
        var partForRepairParts = await _service.Get();
        if (partForRepairParts == null)
        {
            return NotFound();
        }

        return Ok(partForRepairParts);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<PartForRepairPart>> GetPartForRepairPart(int id)
    {
        var partForRepairPart = await _service.Get(id);

        if (partForRepairPart == null)
        {
            return NotFound();
        }

        return Ok(partForRepairPart);
    }

    [RequiresClaim(ClaimTypes.Role, UserRole.ADMIN)]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPartForRepairPart(int id, PartForRepairPartDto partForRepairPartDto)
    {
        var updatedPartForRepairPart = await _service.Update(id, partForRepairPartDto);
        if (updatedPartForRepairPart == null)
        {
            return NotFound();
        }

        return NoContent();
    }

    [RequiresClaim(ClaimTypes.Role, UserRole.ADMIN)]
    [HttpPost]
    public async Task<ActionResult<PartForRepairPart>> PostPartForRepairPart(PartForRepairPartDto partForRepairPartDto)
    {
        var createdPartForRepairPart = await _service.Create(partForRepairPartDto);
        if (createdPartForRepairPart == null)
        {
            return BadRequest();
        }

        return CreatedAtAction("GetPartForRepairPart", new { id = createdPartForRepairPart.Id },
            createdPartForRepairPart);
    }

    [RequiresClaim(ClaimTypes.Role, UserRole.ADMIN)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePartForRepairPart(int id)
    {
        var partForRepairPart = await _service.Get(id);
        if (partForRepairPart == null)
        {
            return NotFound();
        }
        
        await _service.Delete(id);
        
        return Ok(partForRepairPart);
    }
}