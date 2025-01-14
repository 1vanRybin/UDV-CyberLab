using Domain.DTO.Answers;
using Domain.Entities;

namespace Domain.DTO.Questions
{
    public class QuestionUpdateDto
    {
        public QuestionOpenDto? OpenAnswer { get; set; }
        public QuestionVariantDto? VariantAnswer { get; set; }
        public QuestionComplianceDto? ComplianceAnswer { get; set; }
        public QuestionFileDto? FileAnswer { get; set; }
    }
}
