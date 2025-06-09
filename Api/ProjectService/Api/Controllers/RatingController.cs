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
    public class RatingController(IRatingService _ratingService) : ControllerBase
    {
        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetProjectRatingSummary(Guid projectId)
        {
            var userId = UserHelper.GetUserId(HttpContext.Request);
            var summary = await _ratingService.GetProjectRatingSummaryAsync(projectId, userId);
            return Ok(summary);
        }

        [HttpGet("project/{projectId}/ratings")]
        public async Task<IActionResult> GetProjectRatings(Guid projectId)
        {
            var ratings = await _ratingService.GetAllRatingsForProjectAsync(projectId);
            return Ok(ratings);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrUpdateRating([FromBody] RatingCreateDto ratingDto)
        {
            var userId = UserHelper.GetUserId(HttpContext.Request);
            var rating = await _ratingService.CreateOrUpdateRatingAsync(ratingDto, userId);
            return Ok(rating);
        }

        [HttpDelete("project/{projectId}")]
        public async Task<IActionResult> DeleteRating(Guid projectId)
        {
            var userId = UserHelper.GetUserId(HttpContext.Request);
            var result = await _ratingService.DeleteRatingAsync(projectId, userId);
            return Ok(result);
        }
    }
}