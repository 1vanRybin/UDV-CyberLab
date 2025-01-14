﻿using AutoMapper;
using Domain.DTO;
using Domain.DTO.Answers;
using Domain.Entities;
using ExampleCore.Helpers;
using Microsoft.AspNetCore.Mvc;
using Service.interfaces;

namespace Api.Controllers;

[ApiController]
[Route("api/Test")]
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

    [HttpGet("userTest/{id:guid}")]
    [ProducesResponseType(typeof(Test), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Test>> GetUserTest(Guid id)
    {
        var userTest = await _testService.GetUserTestByIdAsync(id);

        if (userTest == null)
        {
            return NotFound();
        }

        return Ok(userTest);
    }

    [HttpGet("{id:guid}/short")]
    [ProducesResponseType(typeof(ShortTestDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ShortTestDto>> GetShortTest(Guid id)
    {
        var test = await _testService.GetByIdShortAsync(id);

        if (test == null)
        {
            return NotFound();
        }

        return Ok(test);
    }

    [HttpPost]
    [ProducesResponseType(typeof(TestDto), StatusCodes.Status201Created)]
    public async Task<ActionResult> CreateTest([FromBody]TestDto test)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var ownerId = UserHelper.GetUserId(HttpContext.Request);
        test.OwnerId = ownerId;
        var domainTest = _mapper.Map<Test>(test);
        var createdTest = await _testService.CreateAsync(domainTest);

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
    
    [HttpGet("my")]
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

    [HttpGet("created")]
    public async Task<ActionResult<ICollection<UserTest>>> GetUserTests()
    {
        var userId = UserHelper.GetUserId(HttpContext.Request);
        var results = await _testService.GetAllUserTestsAsync(userId);

        if (results is null)
        {
            return NotFound();
        }

        return Ok(results);
    }

    [HttpGet("passed")]
    public async Task<ActionResult<ICollection<UserTest>>> GetCompletedTests()
    {
        var userId = UserHelper.GetUserId(HttpContext.Request);
        var results = await _testService.GetCompletedAsync(userId);

        if (results is null)
        {
            return NotFound();
        }

        return Ok(results);
    }

    [HttpGet("{testId:guid}/results")]
    public async Task<ActionResult<ICollection<UserTest>>> GetTestResults(Guid testId)
    {
        var userId = UserHelper.GetUserId(HttpContext.Request);
        var results = await _testService.GetTestResultsAsync(userId, testId);

        if (results is null)
        {
            return NotFound();
        }

        return Ok(results);
    }
    
    [HttpGet("results/{resulId:guid}/preview")]
    public async Task<ActionResult<UserPreviewResultDto>> GetTestPreview([FromRoute] Guid resulId)
    {
        var userId = UserHelper.GetUserId(HttpContext.Request);
        var result = await _testService.GetTestPreviewResult(resulId);
        if (result is null)
        {
            return NotFound();
        }

        if (result.UserId != userId)
        {
            return BadRequest("Тест не этого пользователя");
        }
        
        return Ok(result);
    }

    [HttpGet("statistics/{testId:guid}")]
    public async Task<IActionResult> GetStatistics([FromRoute] Guid testId)
    {
        var result = await _testService.GetTestStatistics(testId);

        return Ok(result);
    }
}
