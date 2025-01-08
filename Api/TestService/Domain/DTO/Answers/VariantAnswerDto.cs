
namespace Domain.DTO.Answers
{
    /// <summary>
    /// DTO для ответа на вопрос типа "QuestionVariant".
    /// </summary>
    public class VariantAnswerDto
    {
        /// <summary>
        /// Идентификатор вопроса (QuestionVariant).
        /// </summary>
        public Guid QuestionId { get; set; }
        
        public Guid UserAnswerId { get; set; }

        /// <summary>
        /// Массив выбранных пользователем вариантов ответа.
        /// </summary>
        public int[] SelectedAnswers { get; set; } = Array.Empty<int>();
    }
}
