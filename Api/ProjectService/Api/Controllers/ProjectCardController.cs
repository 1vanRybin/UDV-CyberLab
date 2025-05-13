using Domain.DTO;
using ExampleCore.Helpers;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectCardController(IProjectService _projectService) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProjectCard(Guid id)
    {
        var card = await _projectService.GetByIdAsync(id);
        return Ok(card);
    }
    [HttpGet("allShort")]
    public async Task<IActionResult> GetAllShortProjectCards()
    {
        var cards = await _projectService.GetAllShortCards();
        return Ok(cards);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProjectCard([FromForm] ProjectCardDTO request)
    {
        var ownerId = UserHelper.GetUserId(HttpContext.Request);

        var guid = await _projectService.CreateAsync(request, request.LogoPhoto, request.ProjectPhoto,
            request.Documentation, ownerId);

        return Ok(guid);
    }
}
