using Domain.DTO.Questions;

namespace Domain.Entities
{
    public record QuestionFileDto : QuestionBaseDto
    {
        public string InputFile { get; set; }
    }
}
