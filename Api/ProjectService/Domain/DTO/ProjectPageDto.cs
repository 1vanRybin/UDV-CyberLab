namespace Domain.DTO
{
    public class ProjectPageDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public string? ShortDescription { get; set; }
        public required string OwnerName { get; set; }
        public required double Rating { get; set; }
        public required string LandingURL { get; set; }
        public int ViewsCount { get; set; }
        public int CommentsCount { get; set; }
    }
}
