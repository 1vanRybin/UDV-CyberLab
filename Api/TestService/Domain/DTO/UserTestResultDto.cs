using Domain.Entities;

namespace Domain.DTO;

public class UserTestResultDto
{
    public Guid TestId { get; set; }
    public string TestName { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public Guid UserId { get; set; }

    public TestState State { get; set; }
    public int AttemptNumber { get; set; }
    public int LeftAttemptsCount { get; set; }
    public float ScoredPoints { get; set; }
    public float MaxPoints { get; set; }
    public DateTime LeftTestTime { get; set; }
    public bool IsChecked { get; set; }
}