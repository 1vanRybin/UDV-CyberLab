using Domain.Interfaces;
using ExampleCore.Dal.Base;

namespace Domain.Entities
{
    public record Test : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public int AttemptsCount { get; set; }

        public DateTime StartTestTime { get; set; }
        public DateTime EndTestTime { get; set; }

        public DateTime PassTestTime { get; set; }

        public List<IQuestionBase> Questions { get; set; }
        public virtual ICollection<UserTest> UserTests { get; set; }
    }
}
