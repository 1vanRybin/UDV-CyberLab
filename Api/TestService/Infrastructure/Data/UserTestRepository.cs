using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class UserTestRepository : IUserTestRepository
    {
        private readonly ApplicationDbContext _context;
        public UserTestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserTest?> GetByUserAndTestAsync(Guid userId, Guid testId)
        {
            return await _context.UserTests
                .FirstOrDefaultAsync(ut => ut.UserId == userId && ut.TestId == testId);
        }

        public async Task CreateAsync(UserTest? userTest)
        {
            await _context.UserTests.AddAsync(userTest);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UserTest? userTest)
        {
            _context.UserTests.Update(userTest);
            await _context.SaveChangesAsync();
        }
        
        public async Task<UserTest?> GetLatestByUserAndTestAsync(Guid userId, Guid testId)
        {
            return await _context.UserTests
                .Where(ut => ut.UserId == userId && ut.TestId == testId)
                .OrderByDescending(ut => ut.AttemptNumber)
                .FirstOrDefaultAsync();
        }
    }
}