using ExampleCore.Dal.Base;

namespace Domain.Entities
{
    public record Rating : BaseEntity<Guid>
    {
        public required Guid ProjectId { get; set; }
        public required Guid UserId { get; set; }
        public required double Value { get; set; } // Rating value (e.g., 1-5)
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public ProjectCard? Project { get; set; }
    }
}