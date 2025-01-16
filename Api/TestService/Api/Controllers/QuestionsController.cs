using AutoMapper;
using Domain.DTO.Questions;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Service.interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace Api.Controllers;

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

    [HttpPost("compliance")]
    [ProducesResponseType(typeof(QuestionComplianceDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateComplianceQuestion([FromBody] QuestionComplianceDto questionDto)
    {
        var question = new QuestionCompliance
        {
            Text = questionDto.Text,
            Description = questionDto.Description,
            Points = questionDto.Points,
            SortOrder = questionDto.SortOrder,
            TestId = questionDto.TestId,
            Compliances= questionDto.Compliances,
            RightCompliances = questionDto.RightCompliances,
            Variants = questionDto.Variants
        };

        var questionId = await _questionService.CreateAsync(question);
        if (questionId == Guid.Empty)
        {
            return NotFound("Test Was Not Found");
        }


        return Created(
            $"/api/Questions/{questionId}",
            _mapper.Map<QuestionComplianceDto>(question));
    }

    [HttpPost("file")]
    [ProducesResponseType(typeof(QuestionFileDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateFileQuestion([FromBody] QuestionFileDto questionDto)
    {
        var question = _mapper.Map<QuestionFile>(questionDto);
        var questionId = await _questionService.CreateAsync(question);

        return Created(
            $"/api/Questions/{questionId}",
            _mapper.Map<QuestionFileDto>(question));
    }

    [HttpPost("open")]
    [ProducesResponseType(typeof(QuestionOpenDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateOpenQuestion([FromBody] QuestionOpenDto questionDto)
    {
        var question = _mapper.Map<QuestionOpen>(questionDto);
        var questionId = await _questionService.CreateAsync(question);

        return Created(
            $"/api/Questions/{questionId}",
            _mapper.Map<QuestionOpenDto>(question));
    }

    [HttpPost("variant")]
    [ProducesResponseType(typeof(QuestionVariantDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateVariantQuestion([FromBody] QuestionVariantDto questionDto)
    {
        var question = _mapper.Map<QuestionVariant>(questionDto);
        var questionId = await _questionService.CreateAsync(question);



        return Created(
            $"/api/Questions/{questionId}",
            _mapper.Map<QuestionVariantDto>(question));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateQuestion([FromBody] QuestionUpdateDto? questionDto)
    {
        if (questionDto is null)
        {
            return BadRequest();
        }

        var res = await _questionService.UpdateAsync(questionDto);

        if (res is null)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
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