namespace Domain.DTO
{
    public class ShortCardDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? OwnerName { get; set; }
        public Guid? OwnerId { get; set; }
        public string? ShortDescription { get; set; }
        public required double Rating { get; set; }
        public required string LogoPath { get; set; }
        public int ViewsCount { get; set; }
        public int CommentsCount { get; set; }
    }
}
