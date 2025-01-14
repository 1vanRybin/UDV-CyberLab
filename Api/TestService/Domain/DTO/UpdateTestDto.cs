namespace Domain.DTO;

public class UpdateTestDto
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string? Description { get; set; }
    public required string Theme { get; set; }
    public required string Difficulty { get; set; }

    public required int AttemptsCount { get; set; }

    public required DateTime? StartTestTime { get; set; }
    public DateTime? EndTestTime { get; set; }
    public required float MaxPoints { get; set; }

    public TimeSpan? PassTestTime { get; set; }
}