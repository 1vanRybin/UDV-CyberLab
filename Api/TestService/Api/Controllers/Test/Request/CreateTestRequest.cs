using Api.Controllers.Question.Request;

namespace Api.Controllers.Test.Request
{
    public class CreateTestRequest
    {
        public string Name { get; set; } = null!;

        public string Description { get; set; }

        public List<CreateQuestionRequest> Questions { get; set; }
    }
}
