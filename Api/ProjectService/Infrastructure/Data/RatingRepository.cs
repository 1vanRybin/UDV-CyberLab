using Domain.Entities;
using Domain.Interfaces;
using Infrastucture.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class RatingRepository(ProjectsDbContext context) : BaseRepository(context), IRatingRepository
{
    public async Task<Rating?> GetUserRatingForProjectAsync(Guid projectId, Guid userId)
    {
        return await context.Ratings
            .FirstOrDefaultAsync(r => r.ProjectId == projectId && r.UserId == userId);
    }

    public async Task<List<Rating>> GetAllRatingsForProjectAsync(Guid projectId)
    {
        return await context.Ratings
            .Where(r => r.ProjectId == projectId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<double> GetAverageRatingForProjectAsync(Guid projectId)
    {
        var avg = await context.Ratings
            .Where(r => r.ProjectId == projectId)
            .Select(r => r.Value)
            .AverageOrDefaultAsync();

        return avg;
    }

    public async Task<int> GetRatingCountForProjectAsync(Guid projectId)
    {
        return await context.Ratings
            .CountAsync(r => r.ProjectId == projectId);
    }
}