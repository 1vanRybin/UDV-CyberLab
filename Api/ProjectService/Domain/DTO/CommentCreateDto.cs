namespace Domain.DTO
{
    public class CommentCreateDto
    {
        public required string Text { get; set; }
        public required Guid ProjectId { get; set; }

        public required string UserName { get; set; }
    }
}