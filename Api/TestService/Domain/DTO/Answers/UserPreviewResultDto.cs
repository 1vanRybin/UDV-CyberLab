using Domain.DTO.Questions;

namespace Domain.DTO.Answers;

public class UserPreviewResultDto
{
    public Guid UserId { get; set; }
    public List<QuestionResultDto> Questions { get; set; } = new List<QuestionResultDto>();
}