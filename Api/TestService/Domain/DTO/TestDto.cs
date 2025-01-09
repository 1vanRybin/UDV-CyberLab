namespace Domain.DTO;

public class TestDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string Theme { get; set; }
    public string Difficulty { get; set; }
    public Guid OwnerId { get; set; }

    public int? AttemptsCount { get; set; }

    public DateTime? StartTestTime { get; set; }
    public DateTime? EndTestTime { get; set; }
    public float MaxPoints { get; set; }

    public TimeSpan? PassTestTime { get; set; }
        
    public ICollection<object>? Questions { get; set; }
}