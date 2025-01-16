using Domain.DTO.Questions;

namespace Domain.DTO.Answers;

public class UserPreviewResultDto
{
    public Guid UserId { get; set; }
    public float MaxPoints { get; set; }
    public float CurrentPoints { get; set; }
    public List<QuestionResultDto> Questions { get; set; } = new List<QuestionResultDto>();
}