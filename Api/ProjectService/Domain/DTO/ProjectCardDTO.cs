
namespace Domain.DTO
{
    public class ProjectCardDTO
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public string? ShortDescription { get; set; }
        public required string OwnerName { get; set; }
        public required double Rating { get; set; }
        public required string LogoPath { get; set; }
        public string? PhotoPath { get; set; }
        public required string LandingURL { get; set; }
        public required string DocumentationPath { get; set; }
        public int ViewsCount { get; set; }
    }
}
