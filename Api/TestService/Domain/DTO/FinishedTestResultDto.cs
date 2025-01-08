using Domain.DTO.Questions;

namespace Domain.DTO;

public class FinishedTestResultDto
{
    public float Score { get; set; }
    public float MaxPoints { get; set; }
    public float Percentage { get; set; }
    public List<QuestionResultDto> Questions { get; set; } = new List<QuestionResultDto>();
}
