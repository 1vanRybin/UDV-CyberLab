using Domain.DTO.Answers;

namespace Domain.DTO
{
    public class UserAnswersDto
    {
        /// <summary>
        /// Id записи в UserTest (указывает, что ответы относятся к конкретному прохождению теста).
        /// </summary>
        public Guid UserTestId { get; set; }
        
        /// <summary>
        /// Список ответов для "QuestionOpen".
        /// </summary>
        public List<OpenAnswerDto>? OpenAnswers { get; set; }

        /// <summary>
        /// Список ответов для "QuestionVariant" (вопросы с вариантами выбора).
        /// </summary>
        public List<VariantAnswerDto>? VariantAnswers { get; set; }

        /// <summary>
        /// Список ответов для "QuestionCompliance" (соответствия).
        /// </summary>
        public List<ComplianceAnswerDto>? ComplianceAnswers { get; set; }

        /// <summary>
        /// Список ответов для "QuestionFile".
        /// </summary>
        public List<FileAnswerDto>? FileAnswers { get; set; }
    }
}
