using Core.ValidateAttributes.Files;
using Microsoft.AspNetCore.Http;

namespace Domain.DTO
{
    public class ProjectCardUpdateDto
    {
        public required Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ShortDescription { get; set; }
        public string? LandingURL { get; set; }

        [AllowedExtensions([".png", ".jpg", ".jpeg"])]
        public IFormFile? LogoPhoto { get; set; }

        [AllowedExtensions([".png", ".jpg", ".jpeg"])]
        public IFormFile? ProjectPhoto { get; set; }

        [AllowedExtensions([".doc", ".docx", ".pdf"])]
        public IFormFile? Documentation { get; set; }
    }
}
