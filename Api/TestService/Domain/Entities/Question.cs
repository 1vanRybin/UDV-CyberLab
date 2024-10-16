using ExampleCore.Dal.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public record Question : BaseEntity<Guid>
    {
        public string Text { get; set; }

        public string Description { get; set; }

        public QuestionType QuestionType { get; set; }

        public string CorrectAnswer { get; set; }

        public string QuestionData { get; set; }
    }
}
