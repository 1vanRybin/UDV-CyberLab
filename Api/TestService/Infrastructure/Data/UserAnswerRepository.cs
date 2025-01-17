using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class UserAnswerRepository : IUserAnswerRepository
{
    private readonly ApplicationDbContext _context;

    public UserAnswerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserAnswer?> GetByUserTestAndQuestionAsync(Guid userTestId, Guid questionId)
    {
        return await _context.Set<UserAnswer>()
           .Where(a => a.UserTestId == userTestId && a.QuestionId == questionId)
           .FirstOrDefaultAsync();
    }
    public async Task<List<UserAnswer>> GetAllByUserTestIdAsync(Guid userTestId)
    {
        return await _context.Set<UserAnswer>()
            .Where(a => a.UserTestId == userTestId)
            .ToListAsync();
    }
    
    public async Task<Guid> CreateOrUpdateAsync(UserAnswer userAnswer)
    {
        if (userAnswer.Id != Guid.Empty)
        {
            var existingAnswer = await _context.UserAnswers.FindAsync(userAnswer.Id);
            if (existingAnswer != null)
            {
                _context.Entry(existingAnswer).CurrentValues.SetValues(userAnswer);
                await _context.SaveChangesAsync();
                return existingAnswer.Id;
            }
        }

        userAnswer.Id = Guid.NewGuid();
        _context.UserAnswers.Add(userAnswer);
        await _context.SaveChangesAsync();
        return userAnswer.Id;
    }
}