
namespace Domain.Interfaces
{
    public interface IQuestionBase
    {
        Guid Id { get; set; }
        Guid TestId { get; set; }
        string Text { get; set; }
        string Description { get; set; }
        int SortOrder { get; set; }
        int Points { get; set; }
    }
}
