﻿using Domain.DTO;

namespace Service.interfaces
{
    public interface ITestPassingService
    {
        Task StartTestAsync(Guid testId, Guid userId);
        Task SaveAnswersAsync(Guid testId, Guid userId, UserAnswersDto userAnswersDto);
        Task<FinishedTestResultDto> FinishTestAsync(Guid testId, Guid userId);
        Task<UserAnswersDto> GetSavedAnswersAsync(Guid testId, Guid userId);
    }
}