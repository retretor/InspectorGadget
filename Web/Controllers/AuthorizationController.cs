﻿using Application.Actions.Authorization.AuthorizeByLogin;
using Application.Actions.Authorization.AuthorizeByToken;
using Application.Actions.Authorization.ChangePassword;
using Application.Actions.Authorization.RemindLogin;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("api/auth")]
// TODO: add roles to the endpoints
public class AuthorizationController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthorizationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("login")]
    public async Task<IActionResult> Login([FromQuery] AuthorizeByLoginQuery request)
    {
        var (result, response) = await _mediator.Send(request);

        if (result.Succeeded == false)
        {
            return BadRequest(result);
        }

        return Ok(response);
    }

    [HttpGet("token")]
    public async Task<IActionResult> Token([FromQuery] AuthorizeByTokenQuery request)
    {
        var (result, response) = await _mediator.Send(request);

        if (result.Succeeded == false)
        {
            return BadRequest(result);
        }

        return Ok(response);
    }

    [HttpGet("remind-login")]
    public async Task<IActionResult> RemindLogin([FromQuery] RemindLoginQuery request)
    {
        var (result, response) = await _mediator.Send(request);

        if (result.Succeeded == false)
        {
            return BadRequest(result);
        }

        return Ok(response);
    }

    [HttpGet("change-password")]
    public async Task<IActionResult> ChangePassword([FromQuery] ChangePasswordQuery request)
    {
        var (result, response) = await _mediator.Send(request);

        if (result.Succeeded == false)
        {
            return BadRequest(result);
        }

        return Ok(response);
    }
}