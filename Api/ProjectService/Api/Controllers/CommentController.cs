using Domain.DTO;
using ExampleCore.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentController(ICommentService _commentService) : ControllerBase
    {
        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetProjectComments(Guid projectId)
        {
            var comments = await _commentService.GetCommentsByProjectIdAsync(projectId);
            return Ok(comments);
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CommentCreateDto commentDto)
        {
            var userId = UserHelper.GetUserId(HttpContext.Request);

            var comment = await _commentService.CreateAsync(commentDto, userId);
            return Ok(comment);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateComment([FromBody] CommentUpdateDto updateDto)
        {
            var userId = UserHelper.GetUserId(HttpContext.Request);
            var updatedComment = await _commentService.UpdateAsync(updateDto, userId);
            return Ok(updatedComment);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            var userId = UserHelper.GetUserId(HttpContext.Request);
            var result = await _commentService.DeleteAsync(id, userId);
            return Ok(result);
        }
    }
}