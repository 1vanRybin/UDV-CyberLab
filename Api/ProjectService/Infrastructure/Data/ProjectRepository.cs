using Domain.Interfaces;
using Infrastucture.Data;

namespace Infrastructure.Data;

public class ProjectRepository(ProjectsDbContext context) : BaseRepository(context), IProjectRepository;