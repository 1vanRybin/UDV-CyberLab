using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Infrastructure.Data;

public class TestRepository : ITestStore
{
    private readonly ApplicationDbContext _context;

    public TestRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Test?> GetByIdAsync(Guid testId)
    {
        var testDbSet = _context.Tests;

        return await testDbSet
            .Include(t => t.Questions)
            .FirstOrDefaultAsync(t => t.Id == testId);
    }

    public async Task<UserTest?> GetByIdAndUserIdShortAsync(Guid testId, Guid userId)
    {
        var testDbSet = _context.UserTests.Include(ut => ut.Test);

        return await testDbSet
            .Where(t => t.TestId == testId && t.UserId == userId)
            .OrderByDescending(t => t.AttemptNumber)
            .FirstOrDefaultAsync();
    }

    public async Task<UserTest?> GetBestScore(Guid testId, Guid userId)
    {
        var testDbSet = _context.UserTests
            .Include(ut => ut.Test);

        return await testDbSet
            .Where(t => t.TestId == testId && t.UserId == userId)
            .OrderByDescending(t => t.ScoredPoints)
            .FirstOrDefaultAsync();
    }

    public async Task<ICollection<UserTest?>> GetUserTestResultsAsync(Guid userId)
    {
        var userTestDbSet = _context.UserTests;

        var result = await userTestDbSet
            .Where(ut => ut.UserId == userId)
            .ToListAsync();

        return result;
    }
    public async Task<ICollection<UserTest?>> GetLastUserTests(Guid userId)
    {
        var userTestDbSet = _context.UserTests
            .Include(ut => ut.Test);

        var lastAttempts = await userTestDbSet
            .Where(ut => ut.UserId == userId)
            .GroupBy(ut => ut.TestId)
            .Select(g => g
                    .OrderByDescending(ut => ut.AttemptNumber)
                    .FirstOrDefault()
            )
            .ToListAsync();

        return lastAttempts;
    }

    public async Task<ICollection<QuestionBase>> GetAllQuestionsByTestIdAsync(Guid testId)
    {

        return await _context.Tests
            .Where(t => t.Id == testId)
            .SelectMany(t => t.Questions)
            .ToListAsync();
    }

    public async Task<List<Test?>> GetAllAsync(string? difficulty = null, string? search = null, string? subject = null)
    {
        IQueryable<Test?> query = _context.Tests
            .Include(t => t.Questions);

        if (!string.IsNullOrEmpty(difficulty))
        {
            query = query.Where(t => t.Difficulty == difficulty);
        }

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(t =>
                EF.Functions.ILike(t.Name, $"%{search}%"));
        }

        if (!string.IsNullOrEmpty(subject))
        {
            query = query.Where(t => t.Theme == subject);
        }

        return await query.ToListAsync();
    }

    public async Task<List<Test?>> GetAllUserTestsAsync(Guid userId)
    {
        return await _context.Tests
            .Where(t => t.OwnerId == userId)
            .Include(t=>t.UserTests)
            .ToListAsync();
    }

    public async Task<List<UserTest?>> GetCompletedAsync(Guid userId)
    {
        var userTestDbSet = _context.UserTests.Include(ut => ut.Test);

        var lastCompletedTests = await userTestDbSet
            .Where(ut =>
                ut.UserId == userId &&
                ut.State == TestState.Completed)
            .GroupBy(ut => ut.TestId)
            .Select(g => g
                .OrderByDescending(ut => ut.ScoredPoints)
                .FirstOrDefault())
            .ToListAsync();

        return lastCompletedTests;
    }

    public async Task<List<UserTest?>> GetTestResultsAsync(Guid userId, Guid testId)
    {
        return await _context.UserTests
            .Where(ut =>
                ut.UserId == userId &&
                ut.TestId == testId &&
                ut.State == TestState.Completed)
            .Include(t => t.Test)
            .ToListAsync();
    }

    public async Task<UserTest?> GetUserTest(Guid resultId)
    {
        return await _context.UserTests
            .Include(ut => ut.Test)
            .Include(ut => ut.Test.Questions)
            .FirstOrDefaultAsync(ut => ut.Id == resultId);
    }

    public async Task<List<UserTest?>> GetTestStatistics(Guid testId)
    {
        return await _context.UserTests
            .Include(ut => ut.Test)
            .Where(ut => ut.TestId == testId)
            .GroupBy(ut => ut.TestId)
            .Select(g => g
                .OrderByDescending(ut => ut.ScoredPoints)
                .FirstOrDefault())
            .ToListAsync();
    }
}