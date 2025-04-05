using Core.ValidateAttributes.Files;
using Microsoft.AspNetCore.Http;

namespace Domain.DTO
{
    public class ProjectCardDTO
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public string? ShortDescription { get; set; }
        public required string OwnerName { get; set; }
        public required string LandingURL { get; set; }

        [AllowedExtensions([".png", ".jpg", ".jpeg"])]
        public required IFormFile LogoPhoto { get; set; }

        [AllowedExtensions([".png", ".jpg", ".jpeg"])]
        public IFormFile? ProjectPhoto { get; set; }

        [AllowedExtensions([".doc", ".docx", ".pdf"])]
        public required IFormFile Documentation { get; set; }
    }
}