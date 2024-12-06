namespace Domain.DTO.Questions;

public record QuestionVariantDto : QuestionBaseDto
{
    public int[] Answers { get; set; }
    public int[] CorrectAnswers { get; set; }
}