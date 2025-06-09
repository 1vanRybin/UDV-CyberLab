using Domain.Entities;

namespace Domain.Interfaces;

public interface IRatingRepository : IStandartStore
{
    Task<Rating?> GetUserRatingForProjectAsync(Guid projectId, Guid userId);
    Task<List<Rating>> GetAllRatingsForProjectAsync(Guid projectId);
    Task<double> GetAverageRatingForProjectAsync(Guid projectId);
    Task<int> GetRatingCountForProjectAsync(Guid projectId);
}