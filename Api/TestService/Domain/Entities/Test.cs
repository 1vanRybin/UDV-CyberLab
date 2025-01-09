using ExampleCore.Dal.Base;
using Newtonsoft.Json;

namespace Domain.Entities;

public record Test : BaseEntity<Guid>
{
    public string Name { get; set; }
    public string Theme { get; set; }
    public string Description { get; set; }
    public Guid OwnerId { get; set; }

    public string Difficulty { get; set; }
    public int AttemptsCount { get; set; }
    public DateTime? StartTestTime { get; set; }
    public DateTime? EndTestTime { get; set; }
    public TimeSpan? PassTestTime { get; set; }
    public float MaxPoints { get; set; }

    public int QuestionsCount => Questions is null ? 0 : Questions.Count;

    public virtual ICollection<QuestionBase>? Questions { get; set; }
    
    [JsonIgnore]
    public virtual ICollection<UserTest> UserTests { get; set; }
}