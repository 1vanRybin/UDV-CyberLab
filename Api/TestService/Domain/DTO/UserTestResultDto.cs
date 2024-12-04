namespace Domain.DTO;

public class UserTestResultDto
{
    public Guid TestId { get; set; }
    public string TestName { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public float Points { get; set; }
    public bool IsChecked { get; set; }
}