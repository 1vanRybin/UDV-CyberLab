namespace Domain.DTO.Questions;

public record QuestionOpenDto : QuestionBaseDto
{
    public string Answer { get; set; }
}