using AutoMapper;
using Domain.Entities;
using ExampleCore.Helpers;
using Microsoft.AspNetCore.Mvc;
using Service.interfaces;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TestController : ControllerBase
{
    private readonly ITestService _testService;
    private readonly IMapper _mapper;

    public TestController(ITestService testService, IMapper mapper)
    {
        _testService = testService;
        _mapper = mapper;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Test>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Test>>> GetTests()
    {
        var tests = await _testService.GetAsync();
        return Ok(tests);
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(Test), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Test>> GetTest(Guid id)
    {
        var test = await _testService.GetByIdAsync(id);
        
        if (test == null)
        {
            return NotFound();
        }

        return Ok(test);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(Test), StatusCodes.Status201Created)]
    public async Task<ActionResult<Test>> CreateTest(Test test)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdTest = await _testService.CreateAsync(test);

        return CreatedAtAction(
            nameof(GetTest),
            new { id = createdTest },
            createdTest);
    }
    
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTest(Test test)
    {
        var updatedTest = await _testService.UpdateAsync(test);
        
        return CreatedAtAction(
            nameof(UpdateTest),
            new { id = updatedTest.Id },
            updatedTest);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTest(Guid id)
    {
        var deletedTest = await _testService.DeleteAsync(id);
        if (deletedTest is null)
        {
            return NotFound();
        }
        
        return NoContent();
    }
    
    [HttpGet("user/results")]
    public async Task<ActionResult<ICollection<UserTest>>> GetUserTestResults()
    {
        var userId = UserHelper.GetUserId(HttpContext.Request);
        var results = await _testService.GetUserTestResultsAsync(userId);
        
        if (results is null)
        {
            return NotFound();
        }
        
        return Ok(results);
    }
}