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
public class EmployeeController : ControllerBase
{
    private readonly IEntityService<Employee, EmployeeDto> _service;

    public EmployeeController(EmployeeService service)
    {
        _service = service;
    }

    [RequiresClaim(ClaimTypes.Role, new[] { UserRole.ADMIN, UserRole.RECEPTIONIST, UserRole.MASTER })]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
    {
        var employees = await _service.Get();
        if (employees == null)
        {
            return NotFound();
        }

        return Ok(employees);
    }

    [RequiresClaim(ClaimTypes.Role, new[] { UserRole.ADMIN, UserRole.RECEPTIONIST, UserRole.MASTER })]
    [HttpGet("{id}")]
    public async Task<ActionResult<Employee>> GetEmployee(int id)
    {
        var employee = await _service.Get(id);

        if (employee == null)
        {
            return NotFound();
        }

        return Ok(employee);
    }

    [RequiresClaim(ClaimTypes.Role, new[] { UserRole.ADMIN })]
    [HttpPost]
    public async Task<ActionResult<Employee>> PostEmployee(EmployeeDto employeeDto)
    {
        var employee = await _service.Create(employeeDto);
        if (employee == null)
        {
            return BadRequest();
        }

        return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
    }
    
    [RequiresClaim(ClaimTypes.Role, new[] { UserRole.ADMIN })]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutEmployee(int id, EmployeeDto employeeDto)
    {
        var updatedEmployee = await _service.Update(id, employeeDto);
        if (updatedEmployee == null)
        {
            return NotFound();
        }

        return NoContent();
    }
    
    [RequiresClaim(ClaimTypes.Role, new[] { UserRole.ADMIN })]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var deletedEmployee = await _service.Get(id);
        if (deletedEmployee == null)
        {
            return NotFound();
        }
        
        await _service.Delete(id);

        return NoContent();
    }
}