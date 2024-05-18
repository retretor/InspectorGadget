using System.Security.Claims;
using Application.Actions.Employee;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using Web.Identity;

namespace Web.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly string _imagePath;

    public EmployeeController(IMediator mediator, IConfiguration configuration)
    {
        _mediator = mediator;
        _imagePath = configuration.GetValue<string>("ImageSettings:ImagePath") ??
                     throw new ArgumentNullException(nameof(configuration));
    }

    [HttpGet("{id}")]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.MASTER, Role.ADMIN, Role.RECEPTIONIST })]
    public async Task<IActionResult> Get(int id)
    {
        var (result, entity) = await _mediator.Send(new GetEmployeeQuery { DbUserId = id });
        return result.Succeeded ? Ok(entity) : BadRequest(result);
    }

    [HttpGet]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.MASTER, Role.ADMIN, Role.RECEPTIONIST })]
    public async Task<IActionResult> GetAll([FromQuery] GetAllEmployeesQuery command)
    {
        var (result, entities) = await _mediator.Send(command);
        return result.Succeeded ? Ok(entities) : BadRequest(result);
    }

    [HttpPost]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.ADMIN })]
    public async Task<IActionResult> Create([FromQuery] CreateEmployeeCommand command)
    {
        var (result, id) = await _mediator.Send(command);
        return result.Succeeded == false ? BadRequest(result) : CreatedAtAction(nameof(Get), new { id }, id);
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
        
        using (var image = await Image.LoadAsync(file.OpenReadStream()))
        {
            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Size = new Size(310, 400),
                Mode = ResizeMode.Pad,
                PadColor = Color.White
            }));

            await image.SaveAsync(filePath, new JpegEncoder());
        }

        var fileUrl = $"{Request.Scheme}://{Request.Host}/images/{imageName}";
        return Ok(new { Url = fileUrl });
    }

    [HttpPut]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.ADMIN })]
    public async Task<IActionResult> Update([FromQuery] UpdateEmployeeCommand command)
    {
        var result = await _mediator.Send(command);
        return result.Succeeded ? Ok() : BadRequest(result);
    }

    [HttpPut("UpdateRole")]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.ADMIN })]
    public async Task<IActionResult> UpdateRole([FromQuery] UpdateEmployeeRoleCommand command)
    {
        var result = await _mediator.Send(command);
        return result.Succeeded ? Ok() : BadRequest(result);
    }

    [HttpDelete("{id}")]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.ADMIN })]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteEmployeeCommand { Id = id });
        return result.Succeeded ? Ok() : BadRequest(result);
    }

    [HttpGet("GetMasterRanking")]
    [RequiresClaim(ClaimTypes.Role, new[] { Role.ADMIN })]
    public async Task<IActionResult> GetMasterRanking([FromQuery] GetMasterRankingQuery command)
    {
        var (result, entities) = await _mediator.Send(command);
        return result.Succeeded ? Ok(entities) : BadRequest(result);
    }
}