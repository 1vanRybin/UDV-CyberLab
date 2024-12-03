namespace Domain.Entities
{
    public record QuestionFile : QuestionBase
    {
        public string InputFile { get; set; }
    }
}
