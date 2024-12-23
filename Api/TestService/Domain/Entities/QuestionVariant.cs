namespace Domain.Entities
{
    public record QuestionVariant : QuestionBase
    {
        public int[] Answers { get; set; }
        public int[] CorrectAnswers { get; set; }
        
        public bool IsMultipleChoice { get; set; }
    }
}
