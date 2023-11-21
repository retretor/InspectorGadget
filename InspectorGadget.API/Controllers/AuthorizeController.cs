using InspectorGadget.DTOs;
using InspectorGadget.Services.AuthServices;
using Microsoft.AspNetCore.Mvc;

namespace InspectorGadget.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthorizeController : ControllerBase
{
    private readonly AuthorizeService _service;

    public AuthorizeController(AuthorizeService service)
    {
        _service = service;
    }

    // POST: api/auth/login
    [HttpPost("login")]
    public async Task<ActionResult<AuthorizeResponse>> Login(AuthorizeRequest request)
    {
        Console.WriteLine($"Logging in user: {request.Login} {request.Password}");
        var response = await _service.Login(request);
        if (response == null)
        {
            return Unauthorized();
        }
        
        return Ok(response);
    }
    
    // POST: api/auth/register
    [HttpPost("register")]
    public async Task<ActionResult<AuthorizeResponse>> Register(RegisterRequest request)
    {
        Console.WriteLine($"Registering user: {request.Login} {request.Password} {request.Role} {request.Name} {request.SecondName} {request.Telephone}");
        var response = await _service.Register(request);
        if (response == null)
        {
            return Unauthorized();
        }
        
        return Ok(response);
    }
}