namespace Domain.DTO
{
    public class ShortCardDto
    {
        public required string Name { get; set; }
        public string? ShortDescription { get; set; }
        public required double Rating { get; set; }
        public required string LogoPath { get; set; }
        public int ViewsCount { get; set; }
    }
}
