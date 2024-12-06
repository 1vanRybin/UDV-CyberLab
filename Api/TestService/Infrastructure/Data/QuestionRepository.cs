using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Service.interfaces;

namespace Infrastructure.Data;

public class QuestionRepository : IQuestionStore
{
    private readonly ApplicationDbContext _context;
    
    public QuestionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<QuestionBase?> GetByIdAsync(Guid id)
    {
        var question = await _context.Set<QuestionBase>()
            .FirstOrDefaultAsync(q => q.Id == id);

        return question;
    }
}