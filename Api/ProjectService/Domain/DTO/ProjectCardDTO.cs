using Microsoft.AspNetCore.Http;

namespace Domain.DTO
{
    public class ProjectCardDTO
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public string? ShortDescription { get; set; }
        public required string OwnerName { get; set; }
        public required IFormFile LogoPhoto { get; set; }
        public IFormFile? PhotoPath { get; set; }
        public required string LandingURL { get; set; }
        public required IFormFile Documentation { get; set; }
    }
}