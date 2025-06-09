using Domain.DTO;
using Domain.Entities;

namespace Domain.Interfaces;

public interface IProjectRepository : IStandartStore
{
    Task<List<ProjectCard>> GetFilteredProjectsAsync(ProjectFilterDto filter);
    Task<List<ProjectCard>> GetUserProjectsAsync(Guid userId);
}