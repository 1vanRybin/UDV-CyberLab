using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

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

    public async Task<ICollection<UserTest>> GetUserTestResultsAsync(Guid userId)
    {
        var userTestDbSet = _context.UserTests;

        var result = await userTestDbSet
            .Where(ut => ut.UserId == userId)
            .ToListAsync();

        return result;
    }

    public async Task<ICollection<QuestionBase>> GetAllQuestionsByTestIdAsync(Guid testId)
    {

        return await _context.Tests
            .Where(t => t.Id == testId)
            .SelectMany(t => t.Questions)
            .ToListAsync();
    }

    public async Task<List<Test?>> GetAllAsync()
    {
        return await _context.Tests
            .Include(t => t.Questions).ToListAsync();
    }
}