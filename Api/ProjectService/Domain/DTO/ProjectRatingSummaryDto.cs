namespace Domain.DTO
{
    public class ProjectRatingSummaryDto
    {
        public required Guid ProjectId { get; set; }
        public required double AverageRating { get; set; }
        public required int TotalRatings { get; set; }
        public double? UserRating { get; set; }
    }
}