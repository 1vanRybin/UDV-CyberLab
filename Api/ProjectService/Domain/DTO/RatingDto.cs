namespace Domain.DTO
{
    public class RatingDto
    {
        public required Guid Id { get; set; }
        public required Guid ProjectId { get; set; }
        public required Guid UserId { get; set; }
        public required double Value { get; set; }
        public required DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}