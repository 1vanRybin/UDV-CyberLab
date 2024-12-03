namespace Domain.Entities
{
    public record QuestionFileDto : QuestionBase
    {
        public string InputFile { get; set; }
    }
}
