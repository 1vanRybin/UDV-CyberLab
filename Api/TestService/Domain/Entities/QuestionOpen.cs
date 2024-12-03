namespace Domain.Entities
{
    public record QuestionOpen : QuestionBase
    {
        public string Answer { get; set; }
    }
}
