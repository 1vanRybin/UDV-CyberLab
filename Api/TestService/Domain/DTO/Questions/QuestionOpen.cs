namespace Domain.Entities
{
    public record QuestionOpenDto : QuestionBase
    {
        public string Answer { get; set; }
    }
}
