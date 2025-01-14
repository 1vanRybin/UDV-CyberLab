using Domain.Interfaces;

namespace Domain.DTO.Questions;

public abstract record QuestionBaseDto : IQuestionBase
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public string Description { get; set; }
    public int SortOrder { get; set; }
    public int Points { get; set; }
    public Guid TestId { get; set; }
}