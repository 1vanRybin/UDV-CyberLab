using CRM.Data.Common.Exceptions;
using Domain.Interfaces;
using Services.Interfaces;

namespace Services.Services;

public class AdminService(IUserStore userStore) : IAdminService
{
    public async Task DeleteUserAsync(Guid userId)
    {
        var user = await userStore.FindByIdAsync(userId);
        if (user == null) throw new NotFoundException($"User with id {userId} not found.");

        await userStore.DeleteAsync(user);
    }
}