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
}