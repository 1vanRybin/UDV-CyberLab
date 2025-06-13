namespace Services.Interfaces;

public interface IAdminService
{
    Task DeleteUserAsync(Guid userId);
}