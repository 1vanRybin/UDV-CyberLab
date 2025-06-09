namespace Domain.DTO
{
    public class CommentUpdateDto
    {
        public required Guid Id { get; set; }
        public required string Text { get; set; }
    }
}