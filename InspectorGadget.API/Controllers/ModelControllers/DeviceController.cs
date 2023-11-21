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
public class DeviceController : ControllerBase
{
    private readonly IEntityService<Device, DeviceDto> _service;

    public DeviceController(DeviceService service)
    {
        _service = service;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Device>>> GetDevices()
    {
        var devices = await _service.Get();
        if (devices == null)
        {
            return NotFound();
        }

        return Ok(devices);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<Device>> GetDevice(int id)
    {
        var device = await _service.Get(id);

        if (device == null)
        {
            return NotFound();
        }

        return Ok(device);
    }

    [RequiresClaim(ClaimTypes.Role, UserRole.ADMIN)]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutDevice(int id, DeviceDto deviceDto)
    {
        var updatedDevice = await _service.Update(id, deviceDto);
        if (updatedDevice == null)
        {
            return NotFound();
        }

        return NoContent();
    }

    [RequiresClaim(ClaimTypes.Role, UserRole.ADMIN)]
    [HttpPost]
    public async Task<ActionResult<Device>> PostDevice(DeviceDto device)
    {
        var createdDevice = await _service.Create(device);
        if (createdDevice == null)
        {
            return BadRequest();
        }

        return CreatedAtAction("GetDevice", new { id = createdDevice.Id }, createdDevice);
    }

    [RequiresClaim(ClaimTypes.Role, UserRole.ADMIN)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDevice(int id)
    {
        var device = await _service.Get(id);
        if (device == null)
        {
            return NotFound();
        }

        await _service.Delete(id);

        return NoContent();
    }
}