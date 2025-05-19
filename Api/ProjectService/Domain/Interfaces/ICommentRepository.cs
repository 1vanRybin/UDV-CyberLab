using Domain.Entities;

namespace Domain.Interfaces;

public interface ICommentRepository : IStandartStore
{
    Task<List<Comment>> GetCommentsByProjectIdAsync(Guid projectId);
}