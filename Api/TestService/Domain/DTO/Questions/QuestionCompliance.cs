
namespace Domain.Entities
{
    public record QuestionComplianceDto : QuestionBase
    {
        public Dictionary<string, string> Compliances { get; set; }
        public Dictionary<string,string> RightCompliances { get; set; }
    }
}
