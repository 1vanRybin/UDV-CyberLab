
using Domain.DTO.Questions;

namespace Domain.DTO
{
    public class UserTestShortDto
    {
        public Guid Id { get; set; }
        public int AttemptNumber { get; set; }
        public string TestName { get; set; }
        public float MaxPoints { get; set; }
        public float ScoredPoints { get; set; }
        public List<QuestionInfoDto> Questions { get; set; }
    }
}
