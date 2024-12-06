
namespace Domain.DTO.Questions;

public record QuestionComplianceDto : QuestionBaseDto
{
    public Dictionary<string, string> Compliances { get; set; }
    public Dictionary<string,string> RightCompliances { get; set; }
}