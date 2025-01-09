using System.ComponentModel.DataAnnotations.Schema;
using Domain.Interfaces;
using ExampleCore.Dal.Base;
using Newtonsoft.Json;

namespace Domain.Entities;

public abstract record QuestionBase : BaseEntity<Guid>, IQuestionBase
{
    [NotMapped]
    public string QuestionTypeName => GetType().Name; 
        
    public string Text { get; set; }
    public string Description { get; set; }
    public int Points { get; set; }
    public int SortOrder { get; set; }
    public Guid TestId { get; set; }
        
    [JsonIgnore]
    public virtual Test Test { get; set; }
}