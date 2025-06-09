using ExampleCore.Dal.Base;

namespace Domain.Entities
{
    public record Comment : BaseEntity<Guid>
    {
        public required string Text { get; set; }
        public required Guid AuthorId { get; set; }
        public required string AuthorName { get; set; }
        public required Guid ProjectId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ProjectCard? Project { get; set; }
    }
}