
using ExampleCore.Dal.Base;

namespace Domain.DTO.Questions;

public record QuestionComplianceDto : QuestionBaseDto
{
    public Dictionary<string, string> Compliances { get; set; }
    public Dictionary<string,string> RightCompliances { get; set; }
    public string VariantsJson { get; set; }
    public Dictionary<string, List<string>> Variants { get; set; }
}