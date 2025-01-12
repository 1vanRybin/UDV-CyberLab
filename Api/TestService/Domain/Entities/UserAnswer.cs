using ExampleCore.Dal.Base;

namespace Domain.Entities
{
    public record UserAnswer : BaseEntity<Guid>
    {
        public Guid UserTestId { get; set; }
        public Guid QuestionId { get; set; }
        
        public string? QuestionText { get; set; }

        public string? AnswerText { get; set; }
        public string? CorrectText { get; set; }

        public int[]? VariantChoices { get; set; }
        public int[]? CorrectChoices { get; set; }

        public Dictionary<string, string>? ComplianceData { get; set; }
        public Dictionary<string, string>? CorrectData { get; set; }

        public string? FileContent { get; set; }
    }

}
