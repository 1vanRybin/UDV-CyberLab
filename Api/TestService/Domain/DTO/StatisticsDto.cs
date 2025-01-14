
namespace Domain.DTO
{
    public class StatisticsDto
    {
        public Guid UserId { get; set; }
        public string TestName { get; set; }
        public float TestPoints { get; set; }
        public float Points { get; set; }
        public Guid UserTestId { get; set; }
    }
}
