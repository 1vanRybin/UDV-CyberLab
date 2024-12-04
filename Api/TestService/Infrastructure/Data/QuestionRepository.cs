using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class QuestionRepository
{
    private readonly ApplicationDbContext _context;
    
    public QuestionRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IReadOnlyList<QuestionBase>> GetByTestIdAsync(Guid testId)
    {
        var questionsContext = _context.Questions;
        
        var questions = await _context.Questions
            .Where(q => q.Id == testId)  // Используем TestId вместо Id для фильтрации по тесту
            .Select(q => new 
            {
                QuestionCompliance = q as QuestionCompliance,
                QuestionFile = q as QuestionFile,
                QuestionOpen = q as QuestionOpen,
                QuestionVariant = q as QuestionVariant
            })
            .ToListAsync();

        return null;
    }
}