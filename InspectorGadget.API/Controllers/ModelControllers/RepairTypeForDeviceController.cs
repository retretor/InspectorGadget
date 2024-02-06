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
public class RepairTypeForDeviceController : ControllerBase
{
    private readonly RepairTypeForDeviceService _service;

    public RepairTypeForDeviceController(RepairTypeForDeviceService service)
    {
        _service = service;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RepairTypeForDevice>>> GetRepairTypeForDevices()
    {
        var repairTypeForDevices = await _service.Get();
        if (repairTypeForDevices == null)
        {
            return NotFound();
        }

        return Ok(repairTypeForDevices);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<RepairTypeForDevice>> GetRepairTypeForDevice(int id)
    {
        var repairTypeForDevice = await _service.Get(id);

        if (repairTypeForDevice == null)
        {
            return NotFound();
        }

        return Ok(repairTypeForDevice);
    }

    [RequiresClaim(ClaimTypes.Role, UserRole.ADMIN)]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutRepairTypeForDevice(int id, RepairTypeForDeviceDto repairTypeForDeviceDto)
    {
        var updatedRepairTypeForDevice = await _service.Update(id, repairTypeForDeviceDto);
        if (updatedRepairTypeForDevice == null)
        {
            return NotFound();
        }

        return NoContent();
    }

    [RequiresClaim(ClaimTypes.Role, UserRole.ADMIN)]
    [HttpPost]
    public async Task<ActionResult<RepairTypeForDevice>> PostRepairTypeForDevice(
        RepairTypeForDeviceDto repairTypeForDeviceDto)
    {
        var createdRepairTypeForDevice = await _service.Create(repairTypeForDeviceDto);
        if (createdRepairTypeForDevice == null)
        {
            return BadRequest();
        }

        return CreatedAtAction("GetRepairTypeForDevice", new { id = createdRepairTypeForDevice.Id },
            createdRepairTypeForDevice);
    }

    [RequiresClaim(ClaimTypes.Role, UserRole.ADMIN)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRepairTypeForDevice(int id)
    {
        var repairTypeForDevice = await _service.Get(id);
        if (repairTypeForDevice == null)
        {
            return NotFound();
        }

        await _service.Delete(id);

        return NoContent();
    }
}