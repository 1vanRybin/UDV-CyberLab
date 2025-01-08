
namespace Domain.DTO.Answers
{
    /// <summary>
    /// DTO для ответа на вопрос типа "QuestionFile".
    /// </summary>
    public class FileAnswerDto
    {
        /// <summary>
        /// Идентификатор вопроса (QuestionFile).
        /// </summary>
        public Guid QuestionId { get; set; }
        
        public Guid UserAnswerId { get; set; }
        
        /// <summary>
        /// Содержимое файла или путь к файлу, в зависимости от того, как реализована загрузка.
        /// </summary>
        public string? UserFileContent { get; set; }
    }
}
