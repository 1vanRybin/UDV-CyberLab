
namespace Domain.Entities
{
    public record QuestionCompliance : QuestionBase
    {
        public Dictionary<string, string> Compliances { get; set; }
        public Dictionary<string,string> RightCompliances { get; set; }
        public string Variants { get; set; }
    }
}
