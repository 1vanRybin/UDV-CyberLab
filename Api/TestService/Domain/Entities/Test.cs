using ExampleCore.Dal.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public record Test : BaseEntity<Guid>
    {
        public string Name { get; set; } = null!;

        public string Description { get; set; }

        public List<Question> Questions { get; set; }
    }
}
