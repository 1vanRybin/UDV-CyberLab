using AutoMapper;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.interfaces;

namespace Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/Questions")]
    [Produces("application/json")]
    public class QuestionsController : ControllerBase
    {
        private IQuestionService _questionService;
        private IMapper _mapper;

        public QuestionsController(IQuestionService questionService, IMapper mapper)
        {
            _mapper = mapper;   
            _questionService = questionService;
        }

        [HttpGet]
        [Route("{id:guid}", Name = nameof(GetQuestion))]
        [ProducesDefaultResponseType]
        [ProducesResponseType(typeof(IQuestionBase), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetQuestion([FromRoute] Guid id)
        {
            var res = await _questionService.GetQuestionByIdAsync(id);

            return Ok(res);
        }
    }
}
