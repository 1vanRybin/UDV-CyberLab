using Domain.DTO;
using ExampleCore.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProjectCardController(IProjectService _projectService) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProjectCard(Guid id)
    {
        var card = await _projectService.GetByIdAsync(id);
        return Ok(card);
    }

    [HttpGet("allShort")]
    public async Task<IActionResult> GetAllShortProjectCards([FromQuery] SortOrder sortOrder = SortOrder.Default,
        [FromQuery] string? searchQuery = null)
    {
        var filter = new ProjectFilterDto
        {
            SortOrder = sortOrder,
            Name = searchQuery
        };

        var filteredProjects = await _projectService.GetFilteredProjectsAsync(filter);
        return Ok(filteredProjects);
    }

    [HttpGet("myCards")]
    public async Task<IActionResult> GetUserCards()
    {
        var currentUserId = UserHelper.GetUserId(HttpContext.Request);

        var cards = await _projectService.GetUserProjects(currentUserId);
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

    [HttpPut]
    public async Task<IActionResult> UpdateProjectCard([FromForm] ProjectCardUpdateDto request)
    {
        var guid = await _projectService.UpdateAsync(request);

        return Ok(guid);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteProjectCard(Guid id)
    {
        var response = await _projectService.DeleteProjectCardAsync(id);

        return response ? Ok("Deleted") : NotFound($"No cards with id {id}");
    }
}