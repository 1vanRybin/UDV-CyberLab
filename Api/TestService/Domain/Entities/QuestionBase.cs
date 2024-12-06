using Domain.Interfaces;
using ExampleCore.Dal.Base;

namespace Domain.Entities
{
    public abstract record QuestionBase : BaseEntity<Guid>, IQuestionBase
    {
        public string QuestionType { get => GetType().Name; }
        public string Text { get; set; }
        public string Description { get; set; }
        public int Points { get; set; }
        public Guid TestId { get; set; }

        public virtual Test Test { get; set; }
    }
}
