using System.Security.Claims;
using Application.Actions.Device;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Web.Identity;

namespace Web.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class DeviceController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly string _imagePath;

    public DeviceController(IMediator mediator, IConfiguration configuration)
    {
        _mediator = mediator;
        _imagePath = configuration.GetValue<string>("ImageSettings:ImagePath") ??
                     throw new ArgumentNullException(nameof(configuration));
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(int id)
    {
        var (result, entity) = await _mediator.Send(new GetDeviceQuery { Id = id });
        return result.Succeeded ? Ok(entity) : BadRequest(result);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll([FromQuery] GetAllDevicesQuery command)
    {
        var (result, entities) = await _mediator.Send(command);
        return result.Succeeded ? Ok(entities) : BadRequest(result);
    }

    [HttpPost]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.ADMIN })]
    public async Task<IActionResult> Create([FromQuery] CreateDeviceCommand command)
    {
        var (result, id) = await _mediator.Send(command);
        return !result.Succeeded ? BadRequest(result) : CreatedAtAction(nameof(Get), new { id }, id);
    }

    [HttpPost("upload")]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.ADMIN })]
    public async Task<IActionResult> UploadImage(IFormFile file, string imageName)
    {
        if (file.Length == 0) return BadRequest("No file uploaded.");

        var filePath = Path.Combine(_imagePath, imageName);

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var fileUrl = $"{Request.Scheme}://{Request.Host}/images/{imageName}";
        return Ok(new { Url = fileUrl });
    }

    [HttpPut]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.ADMIN })]
    public async Task<IActionResult> Update([FromQuery] UpdateDeviceCommand command)
    {
        var result = await _mediator.Send(command);
        return result.Succeeded ? Ok() : BadRequest(result);
    }

    [HttpDelete("{id}")]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.ADMIN })]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteDeviceCommand { Id = id });
        return result.Succeeded ? Ok() : BadRequest(result);
    }

    [HttpGet("RepairTypesInfo")]
    [AllowAnonymous]
    public async Task<IActionResult> GetRepairTypesInfo(int id)
    {
        var (result, entity) = await _mediator.Send(new GetRepairTypesInfoForDeviceQuery
            { EntityId = id });
        return result.Succeeded ? Ok(entity) : BadRequest(result);
    }
}