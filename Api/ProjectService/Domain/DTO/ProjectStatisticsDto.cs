namespace Domain.DTO
{
    public class ProjectStatisticsDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int ViewsCount { get; set; }

        public int LandingVisits { get; set; }

        public double Rating { get; set; }

        public int TotalRatings { get; set; }

        public int CommentsCount { get; set; }
    }
}