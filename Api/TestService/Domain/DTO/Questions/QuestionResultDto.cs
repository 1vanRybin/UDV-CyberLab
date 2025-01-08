namespace Domain.DTO.Questions;

public class QuestionResultDto
{
    public int SortOrder { get; set; }
    public Guid QuestionId { get; set; }
    public string? QuestionText { get; set; }
    public bool IsCorrect { get; set; }
    public string? UserAnswerText { get; set; }
    public string? CorrectAnswerText { get; set; }
    public int[]? UserChoices { get; set; }
    public int[]? CorrectChoices { get; set; }
    public Dictionary<string, string>? UserCompliances { get; set; }
    public Dictionary<string, string>? CorrectCompliances { get; set; }
    public bool HasUserFile { get; set; }
}