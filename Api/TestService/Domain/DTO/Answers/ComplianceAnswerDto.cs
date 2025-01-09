
namespace Domain.DTO.Answers
{
    /// <summary>
    /// DTO для ответа на вопрос типа "QuestionCompliance".
    /// </summary>
    public class ComplianceAnswerDto
    {
        /// <summary>
        /// Идентификатор вопроса (QuestionCompliance).
        /// </summary>
        public Guid QuestionId { get; set; }

        public Guid UserAnswerId { get; set; }
        /// <summary>
        /// Словарь, где ключ – это, например, элемент левой колонки, а значение – выбранный вариант соответствия справа.
        /// </summary>
        public Dictionary<string, string>? UserCompliances { get; set; }
    }
}
