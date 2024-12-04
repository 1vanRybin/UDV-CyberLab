using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class TestRepository: ITestStore
{
    private readonly ApplicationDbContext _context;
    
    public TestRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<ICollection<TestResult>?> GetUserTestResultsAsync(Guid userId)
    {
        var userTestDbSet = _context.UserTests;

        var result = await userTestDbSet
            .Where(ut => ut.UserId == userId)
            .Select(ut => new TestResult()
            {
                TestId = ut.TestId,
                TestName = ut.Test.Name,
                StartTime = ut.Test.StartTestTime,
                EndTime = ut.Test.EndTestTime,
                Points = ut.Points,
                IsChecked = ut.IsChecked
            }).ToListAsync();

        return result;
    }
}