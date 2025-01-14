
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Domain.Entities
{
    public record QuestionCompliance : QuestionBase
    {
        public Dictionary<string, string> Compliances { get; set; }
        public Dictionary<string,string> RightCompliances { get; set; }

        public string VariantsJson
        {
            get => JsonSerializer.Serialize(Variants);
            set => Variants = string.IsNullOrEmpty(value)
                ? new Dictionary<string, List<string>>()
                : JsonSerializer.Deserialize<Dictionary<string, List<string>>>(value);
        }

        [NotMapped]
        public Dictionary<string, List<string>> Variants { get; set; }
    }
}
