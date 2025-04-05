using Domain.DTO;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectCardController(IProjectService _projectService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateProjectCard([FromForm] ProjectCardDTO request)
    {
        var guid = await _projectService.CreateAsync(request, request.LogoPhoto, request.ProjectPhoto,
            request.Documentation);

        return Ok(guid);
    }
}