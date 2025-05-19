namespace Domain.DTO
{
    public class AdminDashboardDto
    {
        public int TotalProjects { get; set; }

        public int TotalComments { get; set; }

        public int TotalViews { get; set; }

        public List<ProjectStatisticsDto> ProjectsStatistics { get; set; } = new();
    }
}