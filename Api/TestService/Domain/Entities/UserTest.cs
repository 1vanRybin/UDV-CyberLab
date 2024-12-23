using ExampleCore.Dal.Base;

namespace Domain.Entities
{
    public record UserTest : BaseEntity<Guid>
    {
        public Guid TestId { get; set; }
        public Guid UserId { get; set; }

        public int LeftAttemptsCount { get; set; }
        public float Points { get; set; }
        public DateTime LeftTestTime { get; set; }
        public bool IsChecked { get; set; }

        public virtual Test Test { get; set; }
    }
}
