using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.interfaces;

namespace Api.Controllers;

[Authorize]
[ApiController]
[Route("api/Questions")]
[Produces("application/json")]
public class QuestionsController : ControllerBase
{
    private readonly IQuestionService _questionService;
    private readonly IMapper _mapper;

    public QuestionsController(IQuestionService questionService, IMapper mapper)
    {
        _questionService = questionService;
        _mapper = mapper;
    }


    [HttpGet("{id:guid}", Name = nameof(GetQuestion))]
    [Authorize(Roles = "Teacher")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(QuestionComplianceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(QuestionFileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(QuestionOpenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(QuestionVariantDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetQuestion([FromRoute] Guid id)
    {
        var question = await _questionService.GetByIdAsync(id);
        if (question is null)
            return NotFound();


        return Ok(question);
    }
    
    [HttpGet("ByTest/{testId:guid}")]
    [Authorize(Roles = "Teacher")]
    [Authorize(Roles = "Admin")]
    public  async Task<IActionResult> GetQuestionsByTestId([FromRoute] Guid testId)
    {
        var questions = await _questionService.GetByTestIdAsync(testId);
        return Ok(questions);
    }
    
    [HttpPost("compliance")]
    [ProducesResponseType(typeof(QuestionComplianceDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateComplianceQuestion([FromBody] QuestionComplianceDto questionDto)
    {
        var question = _mapper.Map<QuestionCompliance>(questionDto);
        var questionId = await _questionService.CreateAsync(question);

        return CreatedAtRoute(
            nameof(CreateComplianceQuestion),
            new { id = questionId },
            _mapper.Map<QuestionComplianceDto>(question));
    }

    [HttpPost("file")]
    [ProducesResponseType(typeof(QuestionFileDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateFileQuestion([FromBody] QuestionFileDto questionDto)
    {
        var question = _mapper.Map<QuestionFile>(questionDto);
        var questionId = await _questionService.CreateAsync(question);

        return CreatedAtRoute(
            nameof(CreateFileQuestion),
            new { id = questionId },
            _mapper.Map<QuestionFileDto>(question));
    }

    [HttpPost("open")]
    [ProducesResponseType(typeof(QuestionOpenDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateOpenQuestion([FromBody] QuestionOpenDto questionDto)
    {
        var question = _mapper.Map<QuestionOpen>(questionDto);
        var questionId = await _questionService.CreateAsync(question);

        return CreatedAtRoute(
            nameof(CreateOpenQuestion),
            new { id = questionId },
            _mapper.Map<QuestionOpenDto>(question));
    }

    [HttpPost("variant")]
    [ProducesResponseType(typeof(QuestionVariantDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateVariantQuestion([FromBody] QuestionVariantDto questionDto)
    {
        var question = _mapper.Map<QuestionVariant>(questionDto);
        var questionId = await _questionService.CreateAsync(question);
        
        return CreatedAtRoute(
            nameof(CreateVariantQuestion),
            new { id = questionId },
            _mapper.Map<QuestionVariantDto>(question));
    }
    
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Teacher")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateQuestion([FromRoute] Guid id, [FromBody] QuestionBase? questionDto)
    {
        if (questionDto is null || id != questionDto.Id)
        {
            return BadRequest();
        }

        var question = _mapper.Map<IQuestionBase>(questionDto);
        var result = await _questionService.UpdateAsync(question);
        if (result is not null)
        {
            return NotFound();
        }

        return NoContent();
    }
    
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Teacher")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteQuestion([FromRoute] Guid id)
    {
        var result = await _questionService.DeleteAsync(id);
        if (result == Guid.Empty)
        {
            return NotFound();
        }

        return NoContent();
    }
}