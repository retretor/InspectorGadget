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
public class AllowedRepairTypesForEmployeeController : ControllerBase
{
    private readonly AllowedRepairTypesForEmployeeService _service;

    public AllowedRepairTypesForEmployeeController(AllowedRepairTypesForEmployeeService service)
    {
        _service = service;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AllowedRepairTypesForEmployee>>> GetAllowedRepairTypesForEmployees()
    {
        var allowedRepairTypesForEmployees = await _service.Get();
        if (allowedRepairTypesForEmployees == null)
        {
            return NotFound();
        }

        return Ok(allowedRepairTypesForEmployees);
    }


    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<AllowedRepairTypesForEmployee>> GetAllowedRepairTypesForEmployee(int id)
    {
        var allowedRepairTypesForEmployee = await _service.Get(id);

        if (allowedRepairTypesForEmployee == null)
        {
            return NotFound();
        }

        return Ok(allowedRepairTypesForEmployee);
    }


    [RequiresClaim(ClaimTypes.Role, UserRole.ADMIN)]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAllowedRepairTypesForEmployee(int id,
        AllowedRepairTypesForEmployeeDto allowedRepairTypesForEmployeeDto)
    {
        var updatedAllowedRepairTypesForEmployee = await _service.Update(id, allowedRepairTypesForEmployeeDto);
        if (updatedAllowedRepairTypesForEmployee == null)
        {
            return NotFound();
        }

        return NoContent();
    }


    [RequiresClaim(ClaimTypes.Role, UserRole.ADMIN)]
    [HttpPost]
    public async Task<ActionResult<AllowedRepairTypesForEmployee>> PostAllowedRepairTypesForEmployee(
        AllowedRepairTypesForEmployeeDto allowedRepairTypesForEmployee)
    {
        var createdAllowedRepairTypesForEmployee = await _service.Create(allowedRepairTypesForEmployee);
        if (createdAllowedRepairTypesForEmployee == null)
        {
            return BadRequest();
        }

        return CreatedAtAction("GetAllowedRepairTypesForEmployee", new { id = createdAllowedRepairTypesForEmployee.Id },
            createdAllowedRepairTypesForEmployee);
    }


    [RequiresClaim(ClaimTypes.Role, UserRole.ADMIN)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAllowedRepairTypesForEmployee(int id)
    {
        var allowedRepairTypesForEmployee = await _service.Get(id);
        if (allowedRepairTypesForEmployee == null)
        {
            return NotFound();
        }

        await _service.Delete(id);

        return NoContent();
    }
}