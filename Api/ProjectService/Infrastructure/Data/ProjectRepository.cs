using Domain.DTO;
using Domain.Entities;
using Domain.Interfaces;
using Infrastucture.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ProjectRepository(ProjectsDbContext context) : BaseRepository(context), IProjectRepository
{
    public async Task<List<ProjectCard>> GetFilteredProjectsAsync(ProjectFilterDto filter)
    {
        IQueryable<ProjectCard> query = context.Cards;

        if (!string.IsNullOrEmpty(filter.Name))
            query = query.Where(p =>
                EF.Functions.ILike(p.Name, $"%{filter.Name}%"));

        query = filter.SortOrder switch
        {
            SortOrder.ByRating => query.OrderByDescending(p => p.Rating),
            SortOrder.ByViews => query.OrderByDescending(p => p.ViewsCount),
            SortOrder.ByNameAsc => query.OrderBy(p => p.Name),
            SortOrder.ByNameDesc => query.OrderByDescending(p => p.Name),
            _ => query.OrderByDescending(p => p.Rating)
        };

        return await query.ToListAsync();
    }

    public async Task<List<ProjectCard>> GetUserProjectsAsync(Guid userId)
    {
        var query = context.Cards.Where(c => c.OwnerId == userId);

        return await query.ToListAsync();
    }
}