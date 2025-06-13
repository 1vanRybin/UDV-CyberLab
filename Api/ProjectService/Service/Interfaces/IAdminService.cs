using Domain.DTO;

namespace Service.Interfaces
{
    public interface IAdminService
    {
        Task<AdminDashboardDto> GetDashboardAsync();

        Task<ProjectStatisticsDto> GetProjectStatisticsAsync(Guid projectId);

        Task<bool> DeleteCommentAsync(Guid commentId);
        Task<bool> DeleteProjectAsync(Guid projectId);

        Task<List<CommentDto>> GetAllCommentsForModerationAsync();
    }
}