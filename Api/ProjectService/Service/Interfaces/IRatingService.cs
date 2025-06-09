using Domain.DTO;

namespace Service.Interfaces;

public interface IRatingService
{
    Task<RatingDto> CreateOrUpdateRatingAsync(RatingCreateDto ratingDto, Guid userId);
    Task<ProjectRatingSummaryDto> GetProjectRatingSummaryAsync(Guid projectId, Guid userId);
    Task<List<RatingDto>> GetAllRatingsForProjectAsync(Guid projectId);
    Task<bool> DeleteRatingAsync(Guid projectId, Guid userId);
}