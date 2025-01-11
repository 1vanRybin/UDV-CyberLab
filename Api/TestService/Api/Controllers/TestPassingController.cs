﻿using Domain.DTO;
using ExampleCore.Helpers;
using Microsoft.AspNetCore.Mvc;
using Service.interfaces;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/TestPassing")]
    public class TestPassingController : ControllerBase
    {
        private readonly ITestPassingService _testPassingService;
        public TestPassingController(ITestPassingService testPassingService)
        {
            _testPassingService = testPassingService;
        }

        [HttpGet("{testId}/answers")]
        public async Task<IActionResult> GetUserSavedAnswers(Guid testId)
        {
            var userId = UserHelper.GetUserId(HttpContext.Request);
            var answersDto = await _testPassingService.GetSavedAnswersAsync(testId, userId);
            return Ok(answersDto);
        }

        [HttpPost("{testId}/start")]
        public async Task<IActionResult> StartTest(Guid testId)
        {
            await _testPassingService.StartTestAsync(testId, UserHelper.GetUserId(HttpContext.Request));
            return Ok("Тест начат");
        }

        [HttpPost("{testId}/answer")]
        public async Task<IActionResult> SaveAnswers(Guid testId, [FromBody] UserAnswersDto userAnswersDto)
        {
            await _testPassingService.SaveAnswersAsync(testId, UserHelper.GetUserId(HttpContext.Request), userAnswersDto);
            return Ok();
        }

        [HttpPost("{testId}/finish")]
        public async Task<IActionResult> FinishTest(Guid testId)
        {
            var score = await _testPassingService.FinishTestAsync(testId, UserHelper.GetUserId(HttpContext.Request));
            return Ok(score);
        }
    }
}
