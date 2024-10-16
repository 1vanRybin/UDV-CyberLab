using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Api.Controllers.Test.Request;
using Api.Controllers.Test.Response;

namespace WebApi.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class TestsController : ControllerBase
{
    private readonly TestsService _testsService;

    public TestsController(TestsService testsService)
    {
        _testsService = testsService;
    }


    [HttpGet]
    [Authorize(Roles = "Admin,Teacher,User")]
    public async Task<IActionResult<List<TestResponse>>> Get()//todo pagincia..
    {
        var tests = await _testsService.Get();
        var res = new List<TestResponse>();
        foreach(var test in tests)
        {
            res.Add(new TestResponse() {
            Description = test.Description,
            Name = test.Name,
            Questions = test.Questions
            });
        }
        return res;
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Teacher,User")]
    public async Task<ActionResult<Test>> GetById(string id)
    {
        var result = await _testsService.GetById(id).ConfigureAwait(false);
        return result.ToActionResult();
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<ActionResult<string>> Post(CreateTestRequest request)
    {
        var test = new Test
        {
            Description = request.Description,
            Name = request.Name,
            Questions = new List<string>()
        };

        foreach (var question in request.Questions)
        {
            var newQuestion = new Question
            {
                Description = question.Description,
                Text = question.Text,
                QuestionData = JsonSerializer.Serialize(question.QuestionData),
                CorrectAnswer = question.CorrectAnswer,
                QuestionType = question.QuestionType
            };
            var questionIdResult = await _questionsService.Create(newQuestion).ConfigureAwait(false);
            if (!questionIdResult.IsSuccess)
                return questionIdResult.ToActionResult();
            test.Questions.Add(questionIdResult.Result);
        }

        var testId = await _testsService.Create(test).ConfigureAwait(false);
        return testId.ToActionResult();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _testsService.Delete(id).ConfigureAwait(false);
        return result.ToActionResult();
    }
}