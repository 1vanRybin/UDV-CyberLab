namespace Domain.Entities;

public class TestResult
{
    public Guid TestId { get; set; }
    public string TestName { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public float Points { get; set; }
    public bool IsChecked { get; set; }
}