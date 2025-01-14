using Domain.DTO.Answers;

namespace Domain.DTO.Questions
{
    public class QuestionUpdateDto
    {
        public Guid Id { get; set; } 
        public OpenAnswerDto? OpenAnswer { get; set; }
        public VariantAnswerDto? VariantAnswer { get; set; }
        public ComplianceAnswerDto? ComplianceAnswer { get; set; }
        public FileAnswerDto? FileAnswer { get; set; }
    }
}
