using System.IdentityModel.Tokens.Jwt;
using InspectorGadget.DTOs;
using InspectorGadget.Services;
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
        var response = await _service.Login(request);
        if (response == null)
        {
            return Unauthorized();
        }
        
        return Ok(response);
    }
}