using Domain.Interfaces;
using ExampleCore.Dal.Base;

namespace Domain.Entities
{
    public record QuestionBase : BaseEntity<Guid>, IQuestionBase
    {
        public string Text { get; set; }
        public string Description { get; set; }
    }
}
