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
public class RepairTypesListController : ControllerBase
{
    private readonly IEntityService<RepairTypesList, RepairTypesListDto> _service;

    public RepairTypesListController(RepairTypesListService service)
    {
        _service = service;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RepairTypesList>>> GetRepairTypesLists()
    {
        var repairTypesLists = await _service.Get();
        if (repairTypesLists == null)
        {
            return NotFound();
        }

        return Ok(repairTypesLists);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<RepairTypesList>> GetRepairTypesList(int id)
    {
        var repairTypesList = await _service.Get(id);

        if (repairTypesList == null)
        {
            return NotFound();
        }

        return Ok(repairTypesList);
    }

    [RequiresClaim(ClaimTypes.Role, UserRole.ADMIN)]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutRepairTypesList(int id, RepairTypesListDto repairTypesListDto)
    {
        var updatedRepairTypesList = await _service.Update(id, repairTypesListDto);
        if (updatedRepairTypesList == null)
        {
            return NotFound();
        }

        return NoContent();
    }

    [RequiresClaim(ClaimTypes.Role, UserRole.ADMIN)]
    [HttpPost]
    public async Task<ActionResult<RepairTypesList>> PostRepairTypesList(RepairTypesListDto repairTypesListDto)
    {
        var createdRepairTypesList = await _service.Create(repairTypesListDto);
        if (createdRepairTypesList == null)
        {
            return BadRequest();
        }

        return CreatedAtAction("GetRepairTypesList", new { id = createdRepairTypesList.Id }, createdRepairTypesList);
    }

    [RequiresClaim(ClaimTypes.Role, UserRole.ADMIN)]
    [HttpDelete("{id}")]
    public async Task<ActionResult<RepairTypesList>> DeleteRepairTypesList(int id)
    {
        var repairTypesList = await _service.Get(id);
        if (repairTypesList == null)
        {
            return NotFound();
        }

        await _service.Delete(id);

        return Ok(repairTypesList);
    }
}