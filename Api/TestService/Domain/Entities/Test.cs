using Domain.Entities;
using ExampleCore.Dal.Base;

public record Test : BaseEntity<Guid>
{
    public string Name { get; set; }
    public string Theme { get; set; }
    public string Description { get; set; }

    public int AttemptsCount { get; set; }
    public DateTime StartTestTime { get; set; }
    public DateTime EndTestTime { get; set; }
    public DateTime PassTestTime { get; set; }

    public int QuestionsCount { get => Questions.Count; }
    public virtual ICollection<QuestionBase> Questions { get; set; }

    public virtual ICollection<UserTest> UserTests { get; set; }
}
