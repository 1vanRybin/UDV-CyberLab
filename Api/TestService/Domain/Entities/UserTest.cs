using ExampleCore.Dal.Base;

namespace Domain.Entities
{
    public record UserTest : BaseEntity<Guid>
    {
        public Guid TestId { get; set; }
        public Guid UserId { get; set; }

        public string TestName { get; set; }
        public TestState State { get; set; }
        public int AttemptNumber { get; set; }
        public int LeftAttemptsCount { get; set; }
        public float ScoredPoints { get; set; }
        public DateTime LeftTestTime { get; set; }
        public bool IsChecked { get; set; }

        public virtual Test Test { get; set; }
    }
}
