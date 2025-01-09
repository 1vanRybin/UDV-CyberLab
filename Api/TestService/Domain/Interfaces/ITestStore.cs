﻿using Domain.Entities;

namespace Domain.Interfaces;

public interface ITestStore
{
    Task<Test?> GetByIdAsync(Guid testId);
    Task<Test?> GetByIdShortAsync(Guid testId);
    Task<ICollection<UserTest?>> GetUserTestResultsAsync(Guid guid);

    Task<ICollection<QuestionBase>> GetAllQuestionsByTestIdAsync(Guid testId);

    Task<List<Test?>> GetAllAsync();
    Task<List<Test?>> GetAllUserTestsAsync(Guid userId);
    Task<List<UserTest?>> GetCompletedAsync(Guid userId);
}