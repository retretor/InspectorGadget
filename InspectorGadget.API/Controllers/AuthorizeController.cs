using System.Security.Claims;
using InspectorGadget.Controllers.ModelControllers;
using InspectorGadget.DTOs;
using InspectorGadget.Identity;
using InspectorGadget.Models;
using InspectorGadget.Services.AuthServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InspectorGadget.Controllers;

[Authorize]
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
    [AllowAnonymous]
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

    // POST: api/auth/auth
    [AllowAnonymous]
    [HttpGet("auth")]
    public async Task<ActionResult<AuthorizeResponse>> Auth(string token)
    {
        var response = await _service.Auth(token);
        if (response == null)
        {
            return Unauthorized();
        }

        return Ok(response);
    }

    // POST: api/auth/register
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<AuthorizeResponse>> RegisterClient(RegisterRequest request)
    {
        if (request.Role != UserRole.CLIENT.ToString()) return BadRequest();
        var response = await _service.Register(request);
        if (response == null)
        {
            return Unauthorized();
        }

        return CreatedAtAction(nameof(ClientController.GetClient), "Client", new { id = response.Id }, response);
    }

    // TODO: возможно убрать возможность регистрации администратора
    // POST: api/auth/register-employee
    [RequiresClaim(ClaimTypes.Role, UserRole.ADMIN)]
    [HttpPost("register-employee")]
    public async Task<ActionResult<AuthorizeResponse>> RegisterEmployee(RegisterRequest request)
    {
        if (request.Role == UserRole.CLIENT.ToString()) return BadRequest();
        var response = await _service.Register(request);
        if (response == null)
        {
            return Unauthorized();
        }

        return CreatedAtAction(nameof(EmployeeController.GetEmployee), "Employee", new { id = response.Id },
            response);
    }

    // POST: api/auth/change-password
    [AllowAnonymous]
    [HttpPost("change-password")]
    public async Task<ActionResult<AuthorizeResponse>> ChangePassword(ChangePasswordRequest request)
    {
        var response = await _service.ChangePassword(request);
        if (response == null)
        {
            return Unauthorized();
        }

        return Ok(response);
    }

    // POST: api/auth/change-info
    [RequiresClaim(ClaimTypes.Role, UserRole.ADMIN)]
    [HttpPost("change-employee-info")]
    public async Task<ActionResult<AuthorizeResponse>> ChangeEmployeeInfo(ChangeInfoRequest request)
    {
        if (request.Role == UserRole.CLIENT.ToString()) return BadRequest();
        var response = await _service.ChangeInfo(request);
        if (response == null)
        {
            return Unauthorized();
        }

        return Ok(response);
    }

    // POST: api/auth/change-info
    [RequiresClaim(ClaimTypes.Role, new[] { UserRole.CLIENT, UserRole.ADMIN, UserRole.RECEPTIONIST })]
    [HttpPost("change-info")]
    public async Task<ActionResult<AuthorizeResponse>> ChangeClientInfo(ChangeInfoRequest request)
    {
        if (request.Role != UserRole.CLIENT.ToString()) return BadRequest();
        var response = await _service.ChangeInfo(request);
        if (response == null)
        {
            return Unauthorized();
        }

        return Ok(response);
    }
}