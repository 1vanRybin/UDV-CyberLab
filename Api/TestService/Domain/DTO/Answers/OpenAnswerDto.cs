
namespace Domain.DTO.Answers
{
    /// <summary>
    /// DTO для ответа на вопрос типа "QuestionOpen".
    /// </summary>
    public class OpenAnswerDto
    {
        /// <summary>
        /// Идентификатор вопроса (QuestionOpen).
        /// </summary>
        public Guid QuestionId { get; set; }
        
        public Guid UserAnswerId { get; set; }

        /// <summary>
        /// Текст ответа пользователя.
        /// </summary>
        public string? AnswerText { get; set; }
    }
}
