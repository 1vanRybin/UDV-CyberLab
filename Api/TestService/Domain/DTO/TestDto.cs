using Domain.Interfaces;

namespace Domain.DTO
{
    public class TestDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }

        public int? AttemptsCount { get; set; }

        public DateTime? StartTestTime { get; set; }
        public DateTime? EndTestTime { get; set; }

        public DateTime? PassTestTime { get; set; }

        public List<IQuestionBase>? Questions { get; set; }
    }
}
