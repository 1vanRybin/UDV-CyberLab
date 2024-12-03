using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Domain.Entities;
using Service.interfaces;

namespace WebApi.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class TestsController : ControllerBase
{
    private readonly ITestService _testsService;

    public TestsController(ITestService testsService)
    {
        _testsService = testsService;
    }


    [HttpGet]
    [Authorize(Roles = "Admin,Teacher,User")]
    public async Task<IActionResult> GetTests()//todo pagincia..
    {
        var tests = await _testsService.GetAsync();
        var res = new List<TestResponse>();
        foreach(var test in tests)
        {
            
        }
        
        return Ok(res);
    }
    
    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Admin,Teacher,User")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var test = await _testsService.GetByIdAsync(id);
        return Ok(test);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> Create(CreateTestRequest request)
    {
        var test = new Test
        {
            Description = request.Description,
            Name = request.Name,
            Questions = request.Questions.Select(q => new QuestionBase()
            {
                Text = q.Text,
                Description = q.Description,
                QuestionType = q.QuestionType,
                CorrectAnswer = q.CorrectAnswer,
                QuestionData = q.QuestionData
            }).ToList()
        };
        var guid = await _testsService.CreateAsync(test);

        return CreatedAtAction(nameof(GetById), new { id = guid }, test);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deletedTest = await _testsService.DeleteAsync(id);
        if (deletedTest is null)
        {
            return NotFound();
        }

        return NoContent();
    }
}