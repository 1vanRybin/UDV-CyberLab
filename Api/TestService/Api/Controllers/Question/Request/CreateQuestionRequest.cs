using Domain.Entities;

namespace Api.Controllers.Question.Request
{
    public class CreateQuestionRequest
    {
        public string Text { get; set; }

        public string Description { get; set; }

        public QuestionType QuestionType { get; set; }

        public string CorrectAnswer { get; set; }

        public string QuestionData { get; set; }
    }
}
