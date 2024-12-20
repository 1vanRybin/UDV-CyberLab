﻿using Domain.DTO;
using Domain.Entities;

namespace Service.interfaces;

public interface ITestService
{
    Task<IEnumerable<TestDto>> GetAsync();

    Task<TestDto> GetByIdAsync(Guid id);

    Task<Guid> CreateAsync(Test test);

    Task<TestDto?> DeleteAsync(Guid id);

    Task<TestDto> UpdateAsync(Test test);

    Task<ICollection<UserTestResultDto>?> GetUserTestResultsAsync(Guid userId);
    Task<ICollection<object>> GetAllQuestionsByTestIdAsync(Guid testId);
}