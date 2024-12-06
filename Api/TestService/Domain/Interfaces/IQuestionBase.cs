
namespace Domain.Interfaces
{
    public interface IQuestionBase
    {
        public Guid TestId { get; set; }
        string Text { get; set; }
        string Description { get; set; }
    }
}
