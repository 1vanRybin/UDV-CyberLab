using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "ADMIN")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var dashboard = await _adminService.GetDashboardAsync();
            return Ok(dashboard);
        }

        [HttpGet("project/{projectId}/statistics")]
        public async Task<IActionResult> GetProjectStatistics(Guid projectId)
        {
            var statistics = await _adminService.GetProjectStatisticsAsync(projectId);
            return Ok(statistics);
        }

        [HttpDelete("project/{projectId}")]
        public async Task<IActionResult> DeleteProject(Guid projectId)
        {
            var result = await _adminService.DeleteProjectAsync(projectId);
            return Ok(result);
        }

        [HttpGet("comments/moderation")]
        public async Task<IActionResult> GetAllCommentsForModeration()
        {
            var comments = await _adminService.GetAllCommentsForModerationAsync();
            return Ok(comments);
        }

        [HttpDelete("comment/{commentId}")]
        public async Task<IActionResult> DeleteComment(Guid commentId)
        {
            var result = await _adminService.DeleteCommentAsync(commentId);
            return Ok(result);
        }
    }
}