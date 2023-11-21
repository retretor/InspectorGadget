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
public class ClientController : ControllerBase
{
    private readonly IEntityService<Client, ClientDto> _service;

    public ClientController(ClientService service)
    {
        _service = service;
    }

    [RequiresClaim(ClaimTypes.Role, new[] { UserRole.ADMIN, UserRole.RECEPTIONIST }, true)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Client>>> GetClients()
    {
        var clients = await _service.Get();
        if (clients == null)
        {
            return NotFound();
        }

        return Ok(clients);
    }

    [RequiresClaim(ClaimTypes.Role, new[] { UserRole.ADMIN, UserRole.RECEPTIONIST }, true)]
    [HttpGet("{id}")]
    public async Task<ActionResult<Client>> GetClient(int id)
    {
        var client = await _service.Get(id);

        if (client == null)
        {
            return NotFound();
        }

        return Ok(client);
    }

    [RequiresClaim(ClaimTypes.Role, new[] { UserRole.ADMIN, UserRole.RECEPTIONIST }, true)]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutClient(int id, ClientDto clientDto)
    {
        var updatedClient = await _service.Update(id, clientDto);
        if (updatedClient == null)
        {
            return NotFound();
        }

        return NoContent();
    }


    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<Client>> PostClient(ClientDto client)
    {
        var createdClient = await _service.Create(client);
        if (createdClient == null)
        {
            return BadRequest();
        }

        return CreatedAtAction("GetClient", new { id = createdClient.Id }, createdClient);
    }


    [RequiresClaim(ClaimTypes.Role, new[] { UserRole.ADMIN, UserRole.RECEPTIONIST }, true)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClient(int id)
    {
        var client = await _service.Get(id);
        if (client == null)
        {
            return NotFound();
        }

        await _service.Delete(id);

        return NoContent();
    }
}