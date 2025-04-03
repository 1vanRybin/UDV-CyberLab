using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class ProjectsDbContext : DbContext
    {
        public ProjectsDbContext(DbContextOptions<ProjectsDbContext> options) : base(options) 
        {
            Database.EnsureCreated();
        }   

        public DbSet<ProjectCard> Cards { get; set; }
    }
}
