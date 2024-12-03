namespace Domain.Entities
{
    public record QuestionVariantDto : QuestionBase
    {
        public int[] Answers { get; set; }
        public int[] CorrectAnswers { get; set; }
    }
}
