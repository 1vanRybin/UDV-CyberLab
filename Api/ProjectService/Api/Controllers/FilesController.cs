using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController(IProjectService _projectService) : ControllerBase
    {

        [HttpGet("{id}/files")]
        public async Task<IActionResult> GetProjectFiles(Guid id)
        {
            try
            {
                var files = await _projectService.GetProjectFilesAsync(id);
                return Ok(files);
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("file")]
        public async Task<IActionResult> GetFile(string path)
        {
            try
            {
                var file = await _projectService.GetProjectFileAsync(path);
                return Ok(file);
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
