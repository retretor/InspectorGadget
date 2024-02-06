using Application.Actions.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("api/auth")]
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
}