using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class UserAnswerRepository : IUserAnswerRepository
    {
        private readonly ApplicationDbContext _context;

        public UserAnswerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserAnswer>> GetAllByUserTestIdAsync(Guid userTestId)
        {
            return await _context.Set<UserAnswer>()
                .Where(a => a.UserTestId == userTestId)
                .ToListAsync();
        }

        public async Task CreateOrUpdateAsync(UserAnswer answer)
        {
            var existing = await _context.Set<UserAnswer>().FindAsync(answer.Id);
            if (existing == null)
            {
                await _context.Set<UserAnswer>().AddAsync(answer);
            }
            else
            {
                existing.AnswerText = answer.AnswerText;
                existing.VariantChoices = answer.VariantChoices;
                existing.ComplianceData = answer.ComplianceData;
                existing.FileContent = answer.FileContent;
            }
            await _context.SaveChangesAsync();
        }
    }
}
