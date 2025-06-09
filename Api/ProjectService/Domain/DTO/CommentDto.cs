namespace Domain.DTO
{
    public class CommentDto
    {
        public required Guid Id { get; set; }
        public required string Text { get; set; }
        public required Guid AuthorId { get; set; }
        public required string AuthorName { get; set; }
        public required Guid ProjectId { get; set; }
        public required DateTime CreatedAt { get; set; }
    }
}